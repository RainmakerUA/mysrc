using System.Drawing;

namespace RM.Shooter.Settings
{
	internal interface ISettings
	{
		ShooterConfig ShooterConfig { get; }

		FrameConfig[] FrameConfigs { get; }

		void Reload();
	}
}