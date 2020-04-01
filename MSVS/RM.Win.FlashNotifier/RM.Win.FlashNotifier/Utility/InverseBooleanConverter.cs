﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace RM.Win.FlashNotifier.Utility
{
	public sealed class InverseBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType == typeof(bool) && value is bool boolValue)
			{
				return !boolValue;
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert(value, targetType, parameter, culture);
		}
	}
}
