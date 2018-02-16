using System.Collections.Generic;
using System.Json;

namespace RM.UzTicket.Lib.Model
{
	public class Station : ModelBase
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Region { get; set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			Id = GetValueOrDefault<int>(obj, "value");
			Title = GetValueOrDefault<string>(obj, "title");
			Region = GetValueOrDefault<string>(obj, "region");
		}

		public override IDictionary<string, string> ToDictionary()
		{
			return new Dictionary<string, string>
						{
							["value"] = Id.ToString(),
							["title"] = Title,
							["region"] = Region
						};
		}

		public override string ToString()
		{
			return $"{Title} ({Id})";
		}
	}
}
