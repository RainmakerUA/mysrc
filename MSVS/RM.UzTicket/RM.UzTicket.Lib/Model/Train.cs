using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;

namespace RM.UzTicket.Lib.Model
{
	public class Train : ModelBase
	{
		[ModelProperty("num")]
		public string Number { get; set; }

		public int Model { get; set; }

		public int Category { get; set; }

		public TimeSpan TravelTime { get; set; }

		public CoachType[] CoachTypes { get; set; }

		public Station SourceStation { get; set; }

		public Station DestinationStation { get; set; }

		public UzTimestamp DepartureTime { get; set; }

		public UzTimestamp ArrivalTime { get; set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			Number = obj["num"].ReadAs<string>();
			Model = obj["model"].ReadAs<int>();
			Category = obj["category"].ReadAs<int>();
			TravelTime = TimeSpan.Parse(obj["travel_time"].ReadAs<string>());
			CoachTypes = FromJsonArray<CoachType>(obj["types"]);
			SourceStation = StationFromJson(obj["from"]);
			DestinationStation = StationFromJson(obj["till"]);
			DepartureTime = FromJson<UzTimestamp>(obj["from"]);
			ArrivalTime = FromJson<UzTimestamp>(obj["till"]);
		}

		public override IDictionary<string, string> ToDictionary()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return $"{Number}: {SourceStation} - {DestinationStation}, {DepartureTime}";
		}

		public string GetInfo()
		{
			var list = new List<string>
							{
								"Train: {0}",
								"Departure time: {1}",
								"Arrival time: {2}",
								"Travel time: {3}",
								"~~~~~~~~~~~~~~~~~~"
							};
			list.AddRange(CoachTypes.Select(ct => ct.ToString()));
			return String.Format(String.Join(Environment.NewLine, list), Number, DepartureTime, ArrivalTime, TravelTime);
		}

		private static Station StationFromJson(JsonValue json)
		{
			var obj = CheckJson(json);
			return new Station
						{
							Id = obj["station_id"].ReadAs<int>(),
							Title = obj["station"].ReadAs<string>()
						};
		}
	}
}
