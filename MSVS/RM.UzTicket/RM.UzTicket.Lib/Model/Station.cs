using System.Json;

namespace RM.UzTicket.Lib.Model
{
	public class Station : ModelBase
	{
		public int ID { get; private set; }

		public string Title { get; private set; }

		public string Region { get; private set; }

		protected override void FromJsonObject(JsonObject obj)
		{
			ID = obj["value"].ReadAs<int>();
			Title = obj["title"].ReadAs<string>();
			Region = obj.GetValueOrDefault<string>("region");
		}

		public override string ToString()
		{
			return $"{Title} ({ID})";
		}

		public static Station Create(int id, string title, string region = null)
		{
			return new Station { ID = id, Title = title, Region = region };
		}
	}
}
