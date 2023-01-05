using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace RM.Lib.Common.Localization.JsonResourceProvider
{
	public abstract class JsonBaseResourceProvider : ILocalizationProvider
	{
		private const string _resNameRegex = @"([a-z]{2}(?:-[a-z]{2})?)(?:\.json)$";

		private static readonly JsonDocumentOptions _jsonDocumentOptions = new ()
																			{
																				AllowTrailingCommas = true,
																				CommentHandling = JsonCommentHandling.Skip
																			};

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

			Key = _resourceAssembly.GetName().Name
						?? throw new ArgumentException($"{nameof(resourceAssembly)} has no name", nameof(resourceAssembly));
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
			
			if (JsonNode.Parse(stream, documentOptions: _jsonDocumentOptions) is JsonObject jsonObject)
			{
				AddStrings(result, String.Empty, jsonObject);
			}

			return result;

			static void AddStrings(IDictionary<string, string> strings, string prefix, JsonObject jObj)
			{
				if (!String.IsNullOrEmpty(prefix))
				{
					prefix += LocalizationManager.KeySeparator;
				}

				foreach (var (key, jNode) in jObj.Select(kv => (kv.Key, kv.Value)))
				{
					if (jNode != null)
					{
						var fullKey = prefix + key;

						switch (jNode)
						{
							case JsonObject jsonObjInner:
								AddStrings(strings, fullKey, jsonObjInner);
								break;
							case JsonValue jValue when jValue.TryGetValue<string>(out var str):
								strings.Add(fullKey, str);
								break;
							default:
								// TODO: Do nothing?
								break;
						}
					}
				}
			}
		}
	}
}
