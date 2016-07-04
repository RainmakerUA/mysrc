using System;
using System.Collections.Generic;
using System.Json;
using RM.UzTicket.Lib.Utils;

namespace RM.UzTicket.Lib.Model
{
	public class UzTimestamp : ModelBase
	{
		public int Timestamp { get; set; }

		public string StrDate { get; set; }

		public DateTime DateTime => DateTimeExtensions.FromUnixTime(Timestamp);

		protected override void FromJsonObject(JsonObject obj)
		{
			Timestamp = obj["date"].ReadAs<int>();
			StrDate = obj["src_date"].ReadAs<string>();
		}

		public override IDictionary<string, string> ToDictionary()
		{
			return new Dictionary<string, string>
						{
							["date"] = Timestamp.ToString(),
							["src_date"] = StrDate
						};
		}

		public override string ToString()
		{
			return DateTime.ToString("dd MMMM HH:mm");
		}
	}
}
