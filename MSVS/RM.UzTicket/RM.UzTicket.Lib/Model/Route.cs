using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;

namespace RM.UzTicket.Lib.Model
{
	public class Route : ModelBase
	{
		public string TrainNumber { get; private set; }

		public IReadOnlyList<RouteItem> Items { get; private set; }

		public override string ToString()
		{
			var stations = Items[0].Station.Title + (Items.Count > 2 ? " [...] " : " - ") + Items[Items.Count - 1].Station.Title;
			return $"{TrainNumber} {stations}";
		}

		public string GetInfo()
		{
			var lines = new List<string> { ToString() };
			lines.AddRange(Items.Select(it => it.ToString()));

			return String.Join(Environment.NewLine, lines);
		}

		protected override void FromJsonObject(JsonObject obj)
		{
			TrainNumber = obj.Get<string>("train");
			Items = FromJsonArray<RouteItem>(obj["list"]);

			Items[0].IsFinal = false;
			Items[Items.Count - 1].IsFinal = true;
		}
	}
}
