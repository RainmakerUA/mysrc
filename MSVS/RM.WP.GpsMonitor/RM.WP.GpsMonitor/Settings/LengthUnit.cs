using System;
using RM.WP.GpsMonitor.Common;

namespace RM.WP.GpsMonitor.Settings
{
	[UnitEnum("Settings_Length")]
	public enum LengthUnit
	{
		[UnitEnumValue]
		Metres = 0,

		[UnitEnumValue(Coefficient = 1 / 0.3048)]
		Feet, // = 0,3048 m

		[UnitEnumValue(Coefficient = 1 / 0.9144)]
		Yards // = 0,9144 m
	}
}
