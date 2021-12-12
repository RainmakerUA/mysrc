using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RM.Lib.Common.Settings.Providers;

namespace RM.Lib.Common.Settings.Serializers
{
	public class JsonFileSerializer : IFileSerializer
	{
		private const string _defaultFileName = "config.json";

		public JsonFileSerializer() : this(_defaultFileName)
		{
		}

		public JsonFileSerializer(string fileName)
		{
			FileName = fileName;
		}

		public string FileName { get; }

		public T? ReadFile<T>(string fileName)
		{
			if (File.Exists(fileName))
			{
				var json = File.ReadAllText(fileName);
				var jObject = JObject.Parse(json);

				return jObject.ToObject<T>();
			}

			return default;
		}

		public void WriteFile(string fileName, object data) => File.WriteAllText(fileName, JObject.FromObject(data).ToString(Formatting.None));
	}
}
