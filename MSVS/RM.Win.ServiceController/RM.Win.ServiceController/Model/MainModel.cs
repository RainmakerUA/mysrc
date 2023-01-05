using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RM.Lib.Common.Localization;
using RM.Lib.Wpf.Common.Commands;
using RM.Lib.Wpf.Common.ViewModel;
using RM.Win.ServiceController.Common;
using RM.Win.ServiceController.Settings;

namespace RM.Win.ServiceController.Model
{
	public sealed class MainModel : BindableBase
	{
		private static readonly string _newLine;
		private static readonly App _app;

		private static readonly ICommand _startCommand;
		private static readonly ICommand _stopCommand;
		private static readonly ICommand _restartCommand;
		private static readonly ICommand _showSettingsCommand;

		private static readonly TypeLocalization _l10n;
		private static readonly Dictionary<string, SettingsModel.RefreshRateInfo[]> _refreshRatesLocalized;

		private readonly UserSettings _userSettings;

		private Service[]? _services;

		static MainModel()
		{
			_newLine = Environment.NewLine;
			_app = App.Current;

			_startCommand = new DelegateCommand<Panel>(StartServices);
			_stopCommand = new DelegateCommand<Panel>(StopServices);
			_restartCommand = new DelegateCommand<Panel>(RestartServices);
			_showSettingsCommand = new DelegateCommand<MainModel>(ShowSettings);

			_l10n = _app.Localization.GetTypeLocalization(typeof(MainModel));
			_refreshRatesLocalized = new Dictionary<string, SettingsModel.RefreshRateInfo[]>();

			Service.SetServiceEnabled = SetServiceEnabledSetting;
		}

		public MainModel()
		{
			_userSettings = _app.UserSettings;
			Geometry = AdjustGeometry(_userSettings.Geometry);
			Refresh();
		}

		public IEnumerable<Service>? Services => _services;

		public Geometry Geometry { get; }

		public ICommand StartCommand => _startCommand;

		public ICommand StopCommand => _stopCommand;

		public ICommand RestartCommand => _restartCommand;

		public ICommand ShowSettingsCommand => _showSettingsCommand;

		public static Action<Exception?>? ErrorAction { get; set; }

		private void Refresh()
		{
			Service.Reset();
			SetProperty(ref _services, _userSettings.Services.Select(CreateService).ToArray(), nameof(Services));
			Service.UpdateTimer(TimeSpan.FromMilliseconds(_userSettings.RefreshInterval));
		}

		private static Geometry AdjustGeometry(Geometry geometry)
		{
			if (geometry.Left.IsDefault() && geometry.Top.IsDefault() && geometry.Width.IsDefault() && geometry.Height.IsDefault())
			{
				geometry.Left = geometry.Top = Double.NaN;
				geometry.Width = geometry.Height = 0.0;
			}

			return geometry;
		}

		private static Service CreateService(KeyValuePair<string, bool> svcPair)
		{
			var (key, value) = svcPair;
			return new Service(key) { IsEnabled = value };
		}

		private static void SetServiceEnabledSetting(string serviceName, bool enabled)
		{
			var services = _app.UserSettings.Services;

			if (services.TryGetValue(serviceName, out var oldEnabled) && oldEnabled != enabled)
			{
				services[serviceName] = enabled;
			}
		}

		private static void ShowSettings(MainModel? model)
		{
			var localization = _app.Localization;
			var languages = localization.SupportedLocales.Select(GetLanguageInfo).ToArray();
			var userSettings = _app.UserSettings;
			var settingsModel = new SettingsModel(languages, GetRefreshRates())
									{
										Autostart = userSettings.LaunchAtStartup,
										RefreshInterval = userSettings.RefreshInterval,
										Lcid = String.IsNullOrEmpty(userSettings.Language)
												? localization.DefaultLocale ?? 9
												: CultureHelper.GetLcid(userSettings.Language),
										Services = String.Join(_newLine, userSettings.Services.Keys)
									};
			var settingsWindow = new SettingsWindow { DataContext = settingsModel };

			if (settingsWindow.ShowDialog() is true)
			{
				userSettings.LaunchAtStartup = settingsModel.Autostart;
				userSettings.RefreshInterval = settingsModel.RefreshInterval;
				userSettings.Language = CultureHelper.GetLanguageCode(settingsModel.Lcid);
				userSettings.Services = String.IsNullOrWhiteSpace(settingsModel.Services)
										? new Dictionary<string, bool>()
										: settingsModel.Services.Split(_newLine, StringSplitOptions.RemoveEmptyEntries)
												.ToDictionary(svcName => svcName.Trim(), _ => true);
				try
				{
					_app.SaveSettings();
					model?.Refresh();
				}
				catch (Exception e)
				{
					ErrorAction?.Invoke(e);
				}
			}

			static SettingsModel.LanguageInfo GetLanguageInfo(int lcid)
			{
				var name = CultureHelper.GetDisplayName(lcid);
				
				return new SettingsModel.LanguageInfo(name, lcid);
			}

			static SettingsModel.RefreshRateInfo[] GetRefreshRates()
			{
				var locale = _app.UserSettings.Language;

				if (String.IsNullOrEmpty(locale) || !_refreshRatesLocalized.TryGetValue(locale, out var rates))
				{
					rates = new[]
							{
								new SettingsModel.RefreshRateInfo(L("Rate.Low"), 5_000),
								new SettingsModel.RefreshRateInfo(L("Rate.Medium"), 1_000),
								new SettingsModel.RefreshRateInfo(L("Rate.Fast"), 500),
								new SettingsModel.RefreshRateInfo(L("Rate.Ultrafast"), 100)
							};

					if (!String.IsNullOrEmpty(locale))
					{
						_refreshRatesLocalized.Add(locale, rates);
					}
				}

				return rates;
			}

			static string L(string key) => _l10n.GetString(key);
		}

		private static async void StartServices(Panel? panel)
		{
			await ExecuteControlActionAsync(panel, Service.StartAllAsync);
		}

		private static async void StopServices(Panel? panel)
		{
			await ExecuteControlActionAsync(panel, Service.StopAllAsync);
		}

		private static async void RestartServices(Panel? panel)
		{
			await ExecuteControlActionAsync(panel, Service.RestartAllAsync);
		}

		private static async Task ExecuteControlActionAsync(UIElement? element, Func<Task> actionAsync)
		{
			try
			{
				if (element != null)
				{
					element.IsEnabled = false;
				}

				await actionAsync();
			}
			catch (Exception? e)
			{
				if (e is AggregateException aggrExc)
				{
					e = aggrExc.GetInnerException();
				}

				ErrorAction?.Invoke(e);
			}
			finally
			{
				if (element != null)
				{
					element.IsEnabled = true;
				}
			}
		}
	}
}
