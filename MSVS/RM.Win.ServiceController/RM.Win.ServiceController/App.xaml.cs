using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using RM.Lib.Common.Localization;
using RM.Lib.Common.Localization.JsonResourceProvider;
using RM.Lib.Common.Settings;
using RM.Lib.Common.Settings.Providers;
using RM.Lib.Common.Settings.Serializers;
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
			Localization = InitializeLocalization();
		}

		public AppSettings AppSettings => _settingsManager.AppSettings.Clone();

		public UserSettings UserSettings => _settingsManager.UserSettings;

		public LocalizationManager Localization { get; }

		public new static App Current => Application.Current as App;

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			_settingsManager.SaveSettings();
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

		private static LocalizationManager InitializeLocalization()
		{
			return new LocalizationManager(new []{ new JsonResourceProvider(Assembly.GetExecutingAssembly(), "resources/lng") }) { DefaultLocale = 9 };
		}
	}
}
