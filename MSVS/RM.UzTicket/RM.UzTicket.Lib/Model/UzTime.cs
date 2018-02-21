using System;
using System.Json;
using RM.UzTicket.Lib.Utils;

namespace RM.UzTicket.Lib.Model
{
	public class UzTime : ModelBase
	{
		public DateTime DateTime { get; private set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			DateTime = DateTimeExtensions.FromUnixTime(obj["sortTime"].ReadAs<int>());
		}

		public override string ToString()
		{
			return DateTime.ToString("dd MMMM HH:mm");
		}
	}
}
