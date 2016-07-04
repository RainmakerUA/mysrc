using System.Collections.Generic;
using System.Json;

namespace RM.UzTicket.Lib.Model
{
	public class Station : ModelBase
	{
		public int Id { get; set; }

		public string Title { get; set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			Id = obj["station_id"].ReadAs<int>();
			Title = obj["title"].ReadAs<string>();
		}

		public override IDictionary<string, string> ToDictionary()
		{
			return new Dictionary<string, string>
						{
							["station_id"] = Id.ToString(),
							["title"] = Title
						};
		}

		public override string ToString()
		{
			return $"{Title} ({Id})";
		}
	}
}
