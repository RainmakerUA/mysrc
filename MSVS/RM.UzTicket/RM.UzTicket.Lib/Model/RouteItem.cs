using System;
using System.Globalization;
using System.Json;

namespace RM.UzTicket.Lib.Model
{
	public class RouteItem : ModelBase
	{
		public Station Station { get; private set; }

		public ValuePair<double> Coordinates { get; private set; }

		public TimeSpan ArrivalTime { get; private set; }

		public TimeSpan DepartureTime { get; private set; }

		public int Distance { get; private set; }

		internal bool? IsFinal { get; set; }

		public override string ToString()
		{
			var timeStr = IsFinal.HasValue
							? (IsFinal.Value ? $"{ArrivalTime,-13:hh\\:mm}" : $"{DepartureTime,13:hh\\:mm}")
							: $"{ArrivalTime:hh\\:mm} - {DepartureTime:hh\\:mm}";
			return $"{Distance,4} {timeStr} {Station.Title}";
		}

		protected override void FromJsonObject(JsonObject obj)
		{
			Station = Station.Create(obj.Get<int>("code"), obj.Get<string>("name"));
			Coordinates = ValuePair.From(obj.Get<double>("lat"), obj.Get<double>("long"));
			ArrivalTime = GetTimeSpan(obj, "arrivalTime");
			DepartureTime = GetTimeSpan(obj, "departureTime");
			Distance = obj.Get<int>("distance");
		}

		private static TimeSpan GetTimeSpan(JsonObject obj, string key)
		{
			return TimeSpan.ParseExact(obj.Get<string>(key), "hh\\:mm", CultureInfo.InvariantCulture);
		}
	}
}
