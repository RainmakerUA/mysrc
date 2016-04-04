using System;
using Windows.UI.Xaml.Data;
using RM.WP.GpsMonitor.Common;
using RM.WP.GpsMonitor.Settings;

namespace RM.WP.GpsMonitor.Converters
{
	public abstract class UnitValueConverter<T> : IValueConverter where T : struct
	{
		private readonly UnitEnumHelper<T> _helper;

		protected UnitValueConverter()
		{
			_helper = new UnitEnumHelper<T>();
		}

		protected abstract T UnitSettings { get; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is double)
			{
				var val = (double)value;

				if (!Double.IsNaN(val))
				{
					var unitEntry = _helper.GetEntryWithText(UnitSettings);
					return $"{val*unitEntry.Coefficient} {unitEntry.NameKey}";
				}

				return ResourceHelper.NoValue;
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}

	public class LengthUnitValueConverter : UnitValueConverter<LengthUnit>
	{
		protected override LengthUnit UnitSettings => AppSettings.Instance.LengthUnit;
	}

	public class SpeedUnitValueConverter : UnitValueConverter<SpeedUnit>
	{
		protected override SpeedUnit UnitSettings => AppSettings.Instance.SpeedUnit;
	}
}
