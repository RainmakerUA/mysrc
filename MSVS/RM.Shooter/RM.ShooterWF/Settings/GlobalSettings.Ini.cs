using System.IO;
using RM.Shooter.Configuration;

namespace RM.Shooter.Settings
{
	partial class GlobalSettings
	{
		private class IniSettings : ISettings
		{
			private readonly string _fileName;

			private HardcodedSettings _cachedSettings;

			public IniSettings(string fileName)
			{
				_fileName = fileName;
			}

			#region Properties

			private HardcodedSettings CachedSettings
			{
				get
				{
					if (_cachedSettings == null)
					{
						Reload();
					}

					return _cachedSettings;
				}
			}

			public ShooterConfig ShooterConfig
			{
				get { return CachedSettings.ShooterConfig; }
			}

			public FrameConfig[] FrameConfigs
			{
				get { return CachedSettings.FrameConfigs; }
			}

			#endregion

			public void Reload()
			{
				if (!File.Exists(_fileName))
				{
					throw new FileNotFoundException("INI-file is not found!", _fileName);
				}

				_cachedSettings = HardcodedSettings.FromIni(new IniFile(_fileName));
			}
		}
	}
}
