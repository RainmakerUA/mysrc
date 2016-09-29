using System;
using RM.WP.GpsMonitor.Common;

namespace RM.WP.GpsMonitor.Settings
{
	public sealed class AppSettings : ObservableObject
	{
		private ISettingsProvider _provider;

		#region Singleton

		private static class SettingsSingletonProvider
		{
			public static readonly AppSettings ProviderInstance = new AppSettings();

			static SettingsSingletonProvider()
			{
			}
		}

		private AppSettings()
		{
		}

		public static AppSettings Instance => SettingsSingletonProvider.ProviderInstance;

		#endregion

		public ISettingsProvider Provider => _provider;

		#region Data Properties

		// int Timeout?
		// int Threshold

		public CoordinateFormat CoordinateFormat
		{
			get
			{
				CheckInitialized();
				return (CoordinateFormat)_provider.Get<int>();
			}
			set
			{
				_provider.Set((int)value);
				OnPropertyChanged();
			}
		}

		public LengthUnit LengthUnit
		{
			get
			{
				CheckInitialized();
				return (LengthUnit)_provider.Get<int>();
			}
			set
			{
				_provider.Set((int)value);
				OnPropertyChanged();
			}
		}

		public SpeedUnit SpeedUnit
		{
			get
			{
				CheckInitialized();
				return (SpeedUnit)_provider.Get<int>();
			}
			set
			{
				_provider.Set((int)value);
				OnPropertyChanged();
			}
		}

		#endregion

		public void Initialize(ISettingsProvider provider)
		{
			_provider = provider;
		}

		public void CheckInitialized()
		{
			if (_provider == null)
			{
				throw new InvalidOperationException("Settings not initialized!");
			}
		}
	}
}
