using System;
using System.Linq;
using RM.Shooter.Configuration;

namespace RM.Shooter.Settings
{
	partial class GlobalSettings
	{
		private class HardcodedSettings : ISettings
		{
			private readonly ShooterConfig _shooterConfig;
			private readonly FrameConfig[] _frameConfigs;

			public HardcodedSettings(ShooterConfig shooterConfig, FrameConfig[] frameConfigs)
			{
				_shooterConfig = shooterConfig;
				_frameConfigs = frameConfigs;
			}

			public ShooterConfig ShooterConfig
			{
				get { return _shooterConfig; }
			}

			public FrameConfig[] FrameConfigs
			{
				get { return (FrameConfig[])_frameConfigs.Clone(); }
			}

			public void Reload()
			{
			}

			public static HardcodedSettings FromIni(IniFile ini)
			{
				const string shooterName = "Shooter";
				const string framelessPrefix = "Frameless_";

				var shooterSection = ini[shooterName];

				var frameSections = ini.Keys.Where(k => k.StartsWith(framelessPrefix, StringComparison.OrdinalIgnoreCase))
											.Select(k => ini[k]);

				return new HardcodedSettings(
										ShooterConfig.FromIni(shooterSection),
										frameSections.Select(FrameConfig.FromIni).ToArray()
									);
			}
		}
	}
}
