using System;
using Windows.UI.Xaml.Data;
using RM.WP.GpsMonitor.Common;

namespace RM.WP.GpsMonitor.Converters
{
	public class CoordinateToStringConverter : IValueConverter
	{
		private const char _degSign = '°';
		private const char _minSign = '\'';
		private const char _secSign = '"';
		private const string _coordFormat = "{0:00}{1}{2:00}{3}{4:00.0}{5} {6} ({7:F6})";

		private const char _markSeparator = '|';
		private const string _markResPrefix = "CoordinateMarks_";

		public string MarkKey { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (targetType == typeof(String)
					&& value is double)
			{
				return FormatCoordinates((double)value);
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		private string FormatCoordinates(double coord)
		{
			if (Double.IsNaN(coord) || Double.IsInfinity(coord))
			{
				return ResourceHelper.NoValue;
			}

			string posMark = null;
			string negMark = null;

			if (!String.IsNullOrEmpty(MarkKey))
			{
				var markRes = ResourceHelper.GetString(_markResPrefix + MarkKey);
				var marks = markRes.Split(_markSeparator);

				if (marks.Length == 1)
				{
					negMark = marks[0];
				}
				else if(marks.Length > 1)
				{
					posMark = marks[0];
					negMark = marks[1];
				}
			}

			var mark = coord > 0 ? posMark : negMark;
			var sec = Math.Abs(coord);

			var deg = Math.Truncate(sec);
			sec = 60.0 * (sec - deg);

			var min = Math.Truncate(sec);
			sec = 60.0 * (sec - min);

			var res = String.Format(_coordFormat, deg, _degSign, min, _minSign, sec, _secSign, mark, coord);
			return res;
		}
	}
}
