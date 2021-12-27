using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RM.Lib.Common.Localization.JsonResourceProvider
{
	public abstract class JsonBaseResourceProvider : ILocalizationProvider
	{
		private const string _resNameRegex = @"([a-z]{2}(?:-;43[a-z]{2})?)(?:\.json)$";

		private readonly Assembly _resourceAssembly;
		private readonly string? _resourcePrefix;
		private readonly object _initializationLock;
		
		private IDictionary<int, IDictionary<string, string>>? _strings;
		private IReadOnlyList<int>? _supportedLocales;

		protected JsonBaseResourceProvider(Assembly? resourceAssembly, string? resourcePrefix)
		{
			_resourceAssembly = resourceAssembly
									?? Assembly.GetEntryAssembly()
									?? throw new ArgumentException($"{nameof(resourceAssembly)} was not specified", nameof(resourceAssembly));
			_resourcePrefix = resourcePrefix;
			_initializationLock = new object();

			Key = _resourceAssembly.GetName().Name;
		}

		public string Key { get; }

		public IReadOnlyList<int> SupportedLocales
		{
			get
			{
				InitializeResources();
				return _supportedLocales!;
			}
		}

		protected abstract string ResourceSeparator { get; }

		public string? GetString(string key, int lcid)
		{
			InitializeResources();
			return _strings!.TryGetValue(lcid, out var locStrings) && locStrings.TryGetValue(key, out var str) ? str : null;
		}

		protected abstract IDictionary<string, Stream> GetStreams(Assembly resAssembly);

		private void InitializeResources()
		{
			if (_supportedLocales is null)
			{
				lock (_initializationLock)
				{
					if (_supportedLocales is null)
					{
						var locales = new List<int>();
						var strings = new Dictionary<int, IDictionary<string, string>>();
						var streams = GetStreams(_resourceAssembly);
						var regexString = _resNameRegex;
						var resourcePrefix = _resourcePrefix;

						if (!String.IsNullOrEmpty(resourcePrefix))
						{
							var separator = ResourceSeparator;

							if (!resourcePrefix.EndsWith(separator, StringComparison.OrdinalIgnoreCase))
							{
								resourcePrefix += separator;
							}

							regexString = Regex.Escape(resourcePrefix) + _resNameRegex;
						}

						var resNameRegex = new Regex(
													regexString,
													RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
													TimeSpan.FromSeconds(30)
												);
						
						foreach (var (name, stream) in streams)
						{
							var match = resNameRegex.Match(name);

							if (match.Success)
							{
								try
								{
									var lcid = CultureHelper.GetLcid(match.Groups[1].Value);

									locales.Add(lcid);
									strings[lcid] = GetStringsFromJson(stream);
								}
								catch (CultureNotFoundException)
								{
								}
							}
						}

						_supportedLocales = locales.AsReadOnly();
						_strings = strings;
					}
				}
			}
		}

		private static Dictionary<string, string> GetStringsFromJson(Stream stream)
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
								strings.Add(fullKey, (string)jToken!);
								break;
						}
					}
				}
			}
		}
	}
}
