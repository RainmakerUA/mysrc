using System;
using System.IO;
using System.Reflection;

namespace RM.Shooter.Settings
{
	internal static partial class GlobalSettings
	{
		private const string _iniFile = "config.ini";

		private static readonly ISettings _hardcodedSettings = new HardcodedSettings
																(
																	ShooterConfig.Default,
																	new FrameConfig[0]
																);

		public static ISettings GetSettings(string iniFile = null)
		{
			if (String.IsNullOrEmpty(iniFile))
			{
				iniFile = GetIniFileName();
			}

			if (File.Exists(iniFile))
			{
				return new IniSettings(iniFile);
			}

			return _hardcodedSettings;
		}


		private static string GetIniFileName()
		{
			var exePath = Path.GetFullPath(Assembly.GetEntryAssembly().Location);
			var exeDir = Path.GetDirectoryName(exePath) ?? String.Empty;

			return Path.Combine(exeDir, _iniFile);
		}
	}
}
