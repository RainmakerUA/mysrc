using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;

namespace RM.UzTicket.Lib.Model
{
	public abstract class ModelBase
	{
		protected abstract void FromJsonObject(JsonObject obj);

		public static T FromJson<T>(JsonValue json) where T : ModelBase, new()
		{
			var obj = CheckJson(json);
			var result = new T();

			result.FromJsonObject(obj);
			return result;
		}

		public static T[] FromJsonArray<T>(JsonValue json) where T : ModelBase, new()
		{
			var array = json as IEnumerable<JsonValue>;
			return array?.Select(FromJson<T>).ToArray();
		}

		protected static JsonObject CheckJson(JsonValue json)
		{
			return json as JsonObject ?? throw new ArgumentException("Argument must be a JsonObject", nameof(json));
		}
	}
}
