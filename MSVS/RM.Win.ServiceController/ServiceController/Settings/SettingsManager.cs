using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using ServiceController.Helpers;

namespace ServiceController.Settings
{
	internal sealed class SettingsManager
	{
		private const string _ext = ".config";
		private static readonly SettingsManager _current;

		private readonly string _filename;
		private Settings _settings;

		static SettingsManager()
		{
			try
			{
				var exeName = Assembly.GetEntryAssembly().Location;
				var cfgName = Path.ChangeExtension(exeName, _ext);
				_current = new SettingsManager(cfgName);
			}
			catch (IOException)
			{
			}
		}

		private SettingsManager(string filename)
		{
			_filename = filename;
			Load();
		}

		public static SettingsManager Current
		{
			get
			{
				return _current;
			}
		}

		public Settings Settings
		{
			get
			{
				return _settings;
			}
			set
			{
				_settings = value ?? new Settings();
				RaiseSettingsChanged();
			}
		}

		public event EventHandler<EventArgs<Settings>> SettingsChanged;

		public void Save()
		{
			SaveSettings(_filename, _settings);
		}

		private void Load()
		{
			_settings = LoadSettings(_filename);
		}

		private void RaiseSettingsChanged()
		{
			var handler = SettingsChanged;
			if (handler != null)
			{
				handler(this, new EventArgs<Settings>(_settings));
			}
		}

		private static void SaveSettings(string filename, Settings settings)
		{
			try
			{
				var ser = new XmlSerializer(typeof(Settings));
				using (var fstr = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					ser.Serialize(fstr, settings);
				}
			}
			catch (IOException)
			{
			}
		}

		private static Settings LoadSettings(string filename)
		{
			try
			{
				var ser = new XmlSerializer(typeof(Settings));
				using (var fstr = File.OpenRead(filename))
				{
					return (Settings)ser.Deserialize(fstr);
				}

			}
			catch (IOException)
			{
				return new Settings();
			}
			catch(Exception)
			{
				return new Settings();
			}
		}
	}
}
