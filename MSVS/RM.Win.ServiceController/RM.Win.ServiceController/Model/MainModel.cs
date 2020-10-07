using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RM.Win.ServiceController.Common;
using RM.Win.ServiceController.Settings;

namespace RM.Win.ServiceController.Model
{
	public sealed class MainModel : BindableBase
	{
		private static readonly App _app = App.Current;

		private static readonly ICommand _startCommand = new DelegateCommand<Panel>(StartServices);
		private static readonly ICommand _stopCommand = new DelegateCommand<Panel>(StopServices);
		private static readonly ICommand _restartCommand = new DelegateCommand<Panel>(RestartServices);

		private readonly UserSettings _userSettings;

		private Service[] _services;

		static MainModel()
		{
			Service.SetServiceEnabled = SetServiceEnabledSetting;
		}

		public MainModel()
		{
			_userSettings = _app.UserSettings;
			Geometry = AdjustGeometry(_userSettings.Geometry);
			Refresh();
		}

		public IEnumerable<Service> Services => _services;

		public Geometry Geometry { get; }

		public ICommand StartCommand => _startCommand;

		public ICommand StopCommand => _stopCommand;

		public ICommand RestartCommand => _restartCommand;

		public static Action<Exception> ErrorAction { get; set; }

		private void Refresh()
		{
			Service.RefreshInterval = _userSettings.RefreshInterval;
			SetProperty(ref _services, _userSettings.Services.ToArray().Select(CreateService).ToArray(), nameof(Services));
		}

		private static Geometry AdjustGeometry(Geometry geometry)
		{
			if (geometry.Left.IsDefault() && geometry.Top.IsDefault() 
				&& geometry.Width.IsDefault() && geometry.Height.IsDefault())
			{
				geometry.Left = geometry.Top = Double.NaN;
				geometry.Width = geometry.Height = 0.0;
			}

			return geometry;
		}

		private static Service CreateService(KeyValuePair<string, bool> svcPair)
		{
			return new Service(svcPair.Key) { IsEnabled = svcPair.Value };
		}

		private static void SetServiceEnabledSetting(string serviceName, bool enabled)
		{
			var services = _app.UserSettings.Services;

			if (services.TryGetValue(serviceName, out var oldEnabled) && oldEnabled != enabled)
			{
				services[serviceName] = enabled;
			}
		}

		private static async void StartServices(Panel panel)
		{
			await ExecuteControlActionAsync(panel, Service.StartAllAsync);
		}

		private static async void StopServices(Panel panel)
		{
			await ExecuteControlActionAsync(panel, Service.StopAllAsync);
		}

		private static async void RestartServices(Panel panel)
		{
			await ExecuteControlActionAsync(panel, Service.RestartAllAsync);
		}

		private static async Task ExecuteControlActionAsync(UIElement element, Func<Task> actionAsync)
		{
			try
			{
				element.IsEnabled = false;
				await actionAsync();
			}
			catch (Exception e)
			{
				if (e is AggregateException aggrExc)
				{
					e = aggrExc.GetInnerException();
				}

				ErrorAction?.Invoke(e);
			}
			finally
			{
				element.IsEnabled = true;
			}
		}
	}
}
