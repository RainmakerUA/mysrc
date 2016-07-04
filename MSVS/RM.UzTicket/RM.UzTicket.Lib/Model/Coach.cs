using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;

namespace RM.UzTicket.Lib.Model
{
	public class Coach : ModelBase
	{
		public bool AllowBonus { get; set; }

		public string Class { get; set; }

		public int TypeId { get; set; }

		public bool HasBedding { get; set; }

		public int Number { get; set; }

		public int PlacesCount { get; set; }

		public IDictionary<string, decimal> Prices { get; set; }

		public decimal ReservePrice { get; set; }

		public string[] Services { get; set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			AllowBonus = obj["allow_bonus"].ReadAs<bool>();
			Class = obj["coach_class"].ReadAs<string>();
			TypeId = obj["coach_type_id"].ReadAs<int>();
			HasBedding = obj["has_bedding"].ReadAs<bool>();
			Number = obj["num"].ReadAs<int>();
			PlacesCount = obj["places_cnt"].ReadAs<int>();
			ReservePrice = (decimal)obj["reserve_price"].ReadAs<int>() / 100;
			Services = (obj["services"] as IEnumerable<JsonValue>).Select(jv => jv.ReadAs<string>()).ToArray();
			Prices = (obj["prices"] as JsonObject).ToDictionary(kv => kv.Key, kv => (decimal)kv.Value.ReadAs<int>() / 100);
		}

		public override IDictionary<string, string> ToDictionary()
		{
			return new Dictionary<string, string>
						{
							["allow_bonus"] = AllowBonus ? "true" : "false",
							["coach_class"] = Class,
							["coach_type_id"] = TypeId.ToString(),
							["has_bedding"] = HasBedding ? "true" : "false",
							["num"] = Number.ToString(),
							["places_cnt"] = PlacesCount.ToString(),
							["reserve_price"] = (100 * ReservePrice).ToString("F0"),
							//["services"] = ?
							//["prices"] = ?
						};
		}

		public string GetInfo()
		{
			var list = new List<string>
							{
								"Coach #{0} class {1}",
								"Places: {2}",
								"Bedding: {3}",
								"Services: {4}",
								"~~~~~~~~~~~~~"
							};
			var services = String.Join("\u0020", Services);
			list.AddRange(Prices.Select(kv => $"{kv.Key}: {kv.Value} UAH"));
			return String.Format(String.Join(Environment.NewLine, list), Number, Class, PlacesCount, HasBedding ? "+" : "-", services);
		}

		public override string ToString()
		{
			return $"Coach {Number}";
		}
	}
}
