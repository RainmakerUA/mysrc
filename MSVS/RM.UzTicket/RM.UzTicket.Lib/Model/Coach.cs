using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;

namespace RM.UzTicket.Lib.Model
{
	public class Coach : ModelBase
	{
		public bool AllowBonus { get; private set; }

		public string Class { get; private set; }

		public string Type { get; private set; }

		public int Railway { get; private set; }

		public bool HasBedding { get; private set; }

		public int Number { get; private set; }

		public int PlacesCount { get; private set; }

		public IDictionary<string, decimal> Prices { get; private set; }

		public decimal ReservePrice { get; private set; }

		public string[] Services { get; private set; }

		public bool ByWishes { get; private set; }

		public bool AirConditioning { get; private set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			AllowBonus = obj["allowBonus"].ReadAs<bool>();
			Class = obj["class"].ReadAs<string>();
			Type = obj.GetValueOrDefault<string>("type") ?? obj.GetValueOrDefault<string>("type_id");
			Railway = obj["railway"].ReadAs<int>();
			HasBedding = obj["hasBedding"].ReadAs<bool>();
			Number = obj["num"].ReadAs<int>();
			PlacesCount = obj["free"].ReadAs<int>();
			ReservePrice = (decimal)obj["reservePrice"].ReadAs<int>() / 100;
			Services = (obj["services"] as IEnumerable<JsonValue>)?.Select(jv => jv.ReadAs<string>()).ToArray();
			Prices = (obj["prices"] as JsonObject)?.ToDictionary(kv => kv.Key, kv => (decimal)kv.Value.ReadAs<int>() / 100);
			ByWishes = obj.GetValueOrDefault<bool>("byWishes");
			AirConditioning = obj.GetValueOrDefault("air", true);
			
			// Railways: 32: П-Зах., 35: Льв., 40: Од., 43: Півд., 45: Придн., 48: Дон.
		}

		public string GetInfo()
		{
			var list = new List<string>
							{
								"Coach #{0} class {1}",
								"Places: {2}",
								"Air Cond.: {3}",
								"Bedding: {4}",
								"Services: {5}",
								"~~~~~~~~~~~~~"
							};
			var services = String.Join("\u0020", Services);
			list.AddRange(Prices.Select(kv => $"{kv.Key}: {kv.Value} UAH"));
			return String.Format(String.Join(Environment.NewLine, list), Number, Class, PlacesCount, AirConditioning ? "+" : "-", HasBedding ? "+" : "-", services);
		}

		public override string ToString()
		{
			return $"Coach {Number}";
		}
	}
}
