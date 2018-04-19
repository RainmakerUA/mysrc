using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;

namespace RM.UzTicket.Lib.Model
{
	public sealed class Train : ModelBase
	{
		public string Number { get; private set; }

		public int Category { get; private set; }

		public TimeSpan TravelTime { get; private set; }

		public CoachType[] CoachTypes { get; private set; }

		public Station SourceStation { get; private set; }

		public Station DestinationStation { get; private set; }

		public UzTime DepartureTime { get; private set; }

		public UzTime ArrivalTime { get; private set; }

		public string TrainSourceStation { get; private set; }

		public string TrainDestinationStation { get; private set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			Number = obj["num"].ReadAs<string>();
			Category = obj["category"].ReadAs<int>();
			TravelTime = TimeSpan.Parse(obj["travelTime"].ReadAs<string>());
			CoachTypes = FromJsonArray<CoachType>(obj["types"]);
			SourceStation = StationFromJson(obj["from"]);
			DestinationStation = StationFromJson(obj["to"]);
			DepartureTime = FromJson<UzTime>(obj["from"]);
			ArrivalTime = FromJson<UzTime>(obj["to"]);
			TrainSourceStation = obj["from"]["stationTrain"].ReadAs<string>();
			TrainDestinationStation = obj["to"]["stationTrain"].ReadAs<string>();
		}

		public override string ToString()
		{
			return $"{Number}: {TrainSourceStation} - {TrainDestinationStation}";
		}

		public string GetInfo()
		{
			var list = new List<string>
							{
								"Train: {0} {1} - {2}",
								"Departure time: {3}",
								"Arrival time: {4}",
								"Travel time: {5}",
								"~~~~~~~~~~~~~~~~~~"
							};
			list.AddRange(CoachTypes.Select(ct => ct.ToString()));
			return String.Format(
							String.Join(Environment.NewLine, list),
							Number, TrainSourceStation, TrainDestinationStation,
							DepartureTime, ArrivalTime, TravelTime
						);
		}

		internal static Train Create(string number, Station from, Station to, DateTime departure, DateTime arrival, CoachType[] types)
		{
			return new Train
						{
							Number = number,
							SourceStation = from,
							DestinationStation = to,
							DepartureTime = UzTime.Create(departure),
							ArrivalTime = UzTime.Create(arrival),
							TrainSourceStation = $"Before_{from.Title}",
							TrainDestinationStation = $"After_{to.Title}",
							TravelTime = arrival - departure,
							CoachTypes = types
						};
		}

		private static Station StationFromJson(JsonValue json)
		{
			var obj = CheckJson(json);
			return Station.Create(Int32.Parse(obj["code"].ReadAs<string>()), obj["station"].ReadAs<string>());
		}
	}
}
