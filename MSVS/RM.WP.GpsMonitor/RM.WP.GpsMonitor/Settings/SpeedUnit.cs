using RM.WP.GpsMonitor.Common;

namespace RM.WP.GpsMonitor.Settings
{
	[UnitEnum("Settings_Speed")]
	public enum SpeedUnit
	{
		[UnitEnumValue]
		MetrePerSec = 0,

		[UnitEnumValue(Coefficient = 3.6)]
		KilometrePerHour,

		[UnitEnumValue(Coefficient = 2.23694)]
		MilePerHour
	}
}
