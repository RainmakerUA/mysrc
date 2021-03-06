﻿using System;

namespace RM.UzTicket.Lib.Utils
{
	public static class DateTimeExtensions
	{
		private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static int GetUnixTime(this DateTime dateTime)
		{
			return (int)(dateTime.ToUniversalTime() - _unixEpoch).TotalSeconds;
		}

		public static string ToRequestString(this DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-dd");
		}

		public static DateTime FromUnixTime(int unixTime)
		{
			return _unixEpoch.AddSeconds(unixTime).ToLocalTime();
		}

		public static int GetUnixTime()
		{
			return DateTime.UtcNow.GetUnixTime();
		}
	}
}
