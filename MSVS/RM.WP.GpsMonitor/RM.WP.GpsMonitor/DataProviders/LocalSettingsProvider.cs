using System.Collections.Generic;
using Windows.Storage;
using RM.WP.GpsMonitor.Common;

namespace RM.WP.GpsMonitor.DataProviders
{
	internal sealed class LocalSettingsProvider : ISettingsProvider
	{
		private readonly IDictionary<string, object> _settings;

		public LocalSettingsProvider()
		{
			_settings = new Dictionary<string, object>();
		}

		#region Public methods

		public void Load()
		{
			var localSettingsValues = ApplicationData.Current.LocalSettings.Values;

			_settings.Clear();

			foreach (var key in localSettingsValues.Keys)
			{
				_settings[key] = localSettingsValues[key];
			}
		}

		public void Save()
		{
			var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
			localSettingsValues.Clear();

			foreach (var key in _settings.Keys)
			{
				localSettingsValues[key] = _settings[key];
			}
		}

		public void Reset()
		{
			ApplicationData.Current.LocalSettings.Values.Clear();
			Load();
		}

		public T Get<T>(string key)
		{
			if (_settings.ContainsKey(key))
			{
				var val = _settings[key];

				if (val is T)
				{
					return (T) val;
				}
			}

			return default(T);
		}

		public void Set<T>(T value, string key)
		{
			Set(key, value);
		}

		public void Set<T>(string key, T value)
		{
			_settings[key] = value;
		}

		#endregion
	}
}
