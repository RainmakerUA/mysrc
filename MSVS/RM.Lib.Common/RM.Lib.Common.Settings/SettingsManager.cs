using System;

namespace RM.Lib.Common.Settings
{
	public sealed class SettingsManager<TUser, TApp> where TUser : class
	{
		private readonly ISettingsProvider<TUser, TApp> _provider;

		private TUser? _userSettings;

		public SettingsManager(ISettingsProvider<TUser, TApp> provider)
		{
			_provider = provider;
			AppSettings = _provider.GetApplicationSettings();
		}

		public TApp AppSettings { get; }

		public TUser UserSettings => _userSettings ??= _provider.GetUserSettings();

		public event EventHandler<EventArgs<TUser>>? SettingsUpdated;

		public void SaveSettings()
		{
			SaveSettings(UserSettings);
		}

		public void SaveSettings(TUser settings)
		{
			_provider.SaveUserSettings(settings);

			SettingsUpdated?.Invoke(this, EventArgs.Create(settings));
		}
	}
}
