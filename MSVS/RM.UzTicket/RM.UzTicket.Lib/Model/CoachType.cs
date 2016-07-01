using System.Collections.Generic;
using System.Json;

namespace RM.UzTicket.Lib.Model
{
	public class CoachType : ModelBase
	{
		public string Title { get; set; }

		public string Letter { get; set; }

		[ModelProperty("places")]
		public int PlacesCount { get; set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			Title = obj["title"].ReadAs<string>();
			Letter = obj["letter"].ReadAs<string>();
			PlacesCount = obj["places"].ReadAs<int>();
		}

		public override IDictionary<string, string> ToDictionary()
		{
			return new Dictionary<string, string>
						{
							["title"] = Title,
							["letter"] = Letter,
							["places"] = PlacesCount.ToString()
						};
		}

		public override string ToString()
		{
			return $"{Letter}: {PlacesCount} ({Title})";
		}
	}
}
