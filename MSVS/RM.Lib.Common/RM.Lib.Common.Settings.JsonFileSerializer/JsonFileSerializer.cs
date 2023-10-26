using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using RM.Lib.Common.Settings.Providers;

namespace RM.Lib.Common.Settings.Serializers
{
	public class JsonFileSerializer : IFileSerializer
	{
		private class DoubleNanToNullConverter : JsonConverter<double>
		{
			public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
				reader.TokenType switch
				{
					JsonTokenType.Null => Double.NaN,
					JsonTokenType.Number => reader.GetDouble(),
					_ => throw new JsonException("Cannot read value as double")
				};

			public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
			{
				if (Double.IsNaN(value))
				{
					writer.WriteNullValue();
				}
				else
				{
					writer.WriteNumberValue(value);
				}
			}
		}

		private const string _defaultFileName = "config.json";

		private static readonly JsonSerializerOptions _options = new ()
																	{
																		PropertyNameCaseInsensitive = true,
																		Converters = { new DoubleNanToNullConverter() }
																	};

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
				return JsonSerializer.Deserialize<T>(json, _options);
			}

			return default;
		}

		public void WriteFile<T>(string fileName, T data) => File.WriteAllText(fileName, JsonSerializer.Serialize(data, _options));
	}
}
