using System.Json;

namespace RM.UzTicket.Lib.Model
{
	public class CoachType : ModelBase
	{
		public string Title { get; private set; }

		public string Letter { get; private set; }

		public int PlacesCount { get; private set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			Title = obj["title"].ReadAs<string>();
			Letter = obj["letter"].ReadAs<string>();
			PlacesCount = obj["places"].ReadAs<int>();
		}

		public override string ToString()
		{
			return $"{Letter}: {PlacesCount} ({Title})";
		}

		internal static CoachType Create(string title, string letter, int placesCount = 0)
		{
			return new CoachType
						{
							Title = title,
							Letter = letter,
							PlacesCount = placesCount
						};
		}
	}
}
