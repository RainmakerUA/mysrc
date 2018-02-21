using System;
using System.Collections.Generic;
using System.Json;
using RM.UzTicket.Lib.Utils;

namespace RM.UzTicket.Lib.Model
{
	public class RouteData : ModelPersistable
	{
		public Station SourceStation { get; private set; }

		public Station DestinationStation { get; private set; }

		public UzTime Date { get; private set; }

		public Train Train { get; private set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			throw new NotImplementedException();
		}

		public override IDictionary<string, string> ToDictionary()
		{
			return new Dictionary<string, string>
					{
						["from"] = SourceStation.ID.ToString(),
						["to"] = DestinationStation.ID.ToString(),
						["date"] = Date.DateTime.ToRequestString(),
						["train"] = Train.Number
					};
		}

		public static RouteData Create(Station sourceStation, Station destinationStation, UzTime date, Train train)
		{
			return new RouteData
					{
						SourceStation = sourceStation,
						DestinationStation = destinationStation,
						Date = date,
						Train = train
					};
		}
	}
}
