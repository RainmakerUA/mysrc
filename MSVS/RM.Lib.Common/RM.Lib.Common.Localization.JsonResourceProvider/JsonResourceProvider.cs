using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RM.Lib.Common.Localization.JsonResourceProvider
{
	public class JsonResourceProvider : ILocalizationProvider
	{
		private const string _resSeparator = "/";
		private const string _resNameRegex = @"([a-z]{2}(?:-\[a-z]{2})?)(?:\.json)$";

		private readonly IDictionary<int, IDictionary<string, string>> _strings;

		public JsonResourceProvider(string assemblyName, Stream resourceStream, string? resourceEntryPrefix = null)
		{
			_strings = new Dictionary<int, IDictionary<string, string>>();
			Key = assemblyName;
			SupportedLocales = FillStrings(_strings, resourceStream, resourceEntryPrefix);
		}

		public JsonResourceProvider(Assembly resourceAssembly, string? resourceEntryPrefix = null)
			: this(resourceAssembly.GetName().Name, GetAssemblyFirstResourceStream(resourceAssembly), resourceEntryPrefix)
		{
		}

		public string Key { get; }

		public IReadOnlyList<int> SupportedLocales { get; }

		public string? GetString(string key, int lcid)
		{
			return _strings.TryGetValue(lcid, out var localeStrings) && localeStrings.TryGetValue(key, out var str) ? str : null;
		}

		private static Stream GetAssemblyFirstResourceStream(Assembly assembly)
		{
			var resName = assembly.GetManifestResourceNames().FirstOrDefault()
							?? throw new ArgumentException($"Assembly {assembly.GetName().Name} does not contain resources!");
			return assembly.GetManifestResourceStream(resName)!;
		}

		private static IReadOnlyList<int> FillStrings(IDictionary<int, IDictionary<string, string>> strings, Stream resStream, string? prefix)
		{
			var locales = new List<int>();
			var regexString = _resNameRegex;

			if (!String.IsNullOrEmpty(prefix))
			{
				if (!prefix.EndsWith(_resSeparator))
				{
					prefix += _resSeparator;
				}

				regexString = Regex.Escape(prefix) + _resNameRegex;
			}

			var regex = new Regex(regexString, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
			
			using (var resReader = new ResourceReader(resStream))
			{
				foreach (var (name, stream) in resReader.Cast<DictionaryEntry>().Select(de => (de.Key as string, de.Value as Stream)))
				{
					var match = regex.Match(name!);

					if (match.Success)
					{
						try
						{
							var lcid = new CultureInfo(match.Groups[1].Value).LCID;
							locales.Add(lcid);
							strings[lcid] = GetStringsFromJson(stream!);
						}
						catch (CultureNotFoundException)
						{
						}
					}
				}
			}

			return locales.AsReadOnly();
		}

		private static IDictionary<string, string> GetStringsFromJson(Stream stream)
		{
			var result = new Dictionary<string, string>();

			using var textReader = new StreamReader(stream);
			using var jReader = new JsonTextReader(textReader);

			var jObject = JObject.Load(jReader);
			AddStrings(result, String.Empty, jObject);

			return result;

			static void AddStrings(IDictionary<string, string> strings, string prefix, JObject jObj)
			{
				if (!String.IsNullOrEmpty(prefix))
				{
					prefix += LocalizationManager.KeySeparator;
				}

				foreach (var (key, jToken) in jObj!.Select((KeyValuePair<string, JToken> kv) => (kv.Key, kv.Value)))
				{
					if (jToken != null)
					{
						var fullKey = prefix + key;

						switch (jToken.Type)
						{
							case JTokenType.Object:
								AddStrings(strings, fullKey, (jToken as JObject)!);
								break;
							case JTokenType.None:
							case JTokenType.Null:
							case JTokenType.Array:
							case JTokenType.Constructor:
								// Do nothing?
								break;
							default:
								strings.Add(fullKey, (string) jToken!);
								break;
						}
					}
				}
			}
		}
	}
}
