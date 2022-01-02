using System;
using System.Collections.Generic;
using System.Windows;
using RM.Lib.Common;
using RM.Lib.Common.Localization;
using RM.Lib.Common.Localization.JsonResourceProvider;
using RM.Lib.Common.Settings;
using RM.Lib.Common.Settings.Providers;
using RM.Lib.Common.Settings.Serializers;
using RM.Win.ServiceController.Common;
using RM.Win.ServiceController.Settings;

namespace RM.Win.ServiceController
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly SettingsManager<UserSettings, AppSettings> _settingsManager;

		public App()
		{
			var provider = new DefaultFileProvider<UserSettings, AppSettings, JsonFileSerializer>(
																							appName: "ServiceController",
																							useAppData: false,
																							defaultAppSettings: GetDefaultSettings
																						);
			_settingsManager = new SettingsManager<UserSettings, AppSettings>(provider);
			_settingsManager.SettingsUpdated += OnSettingsSaved;

			Localization = InitializeLocalization();
		}

		public AppSettings AppSettings => _settingsManager.AppSettings.Clone();

		public UserSettings UserSettings => _settingsManager.UserSettings;
		
		public LocalizationManager Localization { get; }

		public new static App Current => (Application.Current as App)!;

		public void SaveSettings() => _settingsManager.SaveSettings();

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			SaveSettings();
		}

		private static AppSettings GetDefaultSettings()
		{
			return new AppSettings
						{
							UseLocalAppData = false,
							LaunchAtStartup = false,
							Language = "en",
							Services = new Dictionary<string, bool> { ["TermService"] = true },
							RefreshInterval = 1000,
						};
		}

		private static void OnSettingsSaved(object? sender, EventArgs<UserSettings> args)
		{
			var language = args.Data.Language;

			if (!String.IsNullOrEmpty(language))
			{
				Current.Localization.CurrentUICulture = CultureHelper.Get(language);
			}

			RegistryHelper.RegisterExecutableForStartup(!args.Data.LaunchAtStartup);
		}

		private static LocalizationManager InitializeLocalization()
		{
			LocalizationManager.Initialize(new []{ new JsonResourceProvider(resourceEntryPrefix: "resources/lng") });

			var locMgr = LocalizationManager.Instance;

			locMgr.DefaultLocale = 9;

			return locMgr;
		}
	}
}
