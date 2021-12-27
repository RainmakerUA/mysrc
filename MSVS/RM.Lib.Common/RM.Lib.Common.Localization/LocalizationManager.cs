using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace RM.Lib.Common.Localization
{
	public sealed class LocalizationManager
	{
		private const string _assemblyPrefix = "assembly";
		private const string _missingStringFormat = "[!!!] {2}|{0}|{1}";

		public static readonly string KeySeparator = Type.Delimiter.ToString();

		private readonly IReadOnlyDictionary<string, ILocalizationProvider> _providers;
		private readonly bool _throwOnMissing;
		private readonly bool _enableFallbackLocale;
		private IReadOnlyList<int>? _supportedLocales;

		private CultureInfo? _currentUICulture;

		public LocalizationManager(IReadOnlyList<ILocalizationProvider> providers, bool throwOnMissing = false, bool enableFallbackLocale = true)
		{
			_providers = providers != null ? GetProviderKeys(providers) : throw new ArgumentNullException(nameof(providers));
			_throwOnMissing = throwOnMissing;
			_enableFallbackLocale = enableFallbackLocale;
		}

		public CultureInfo CurrentUICulture
		{
			get => _currentUICulture ?? (DefaultLocale.HasValue ? CultureHelper.Get(DefaultLocale.Value) : Thread.CurrentThread.CurrentUICulture);
			set => _currentUICulture = value;
		}

		public IReadOnlyList<int> SupportedLocales => _supportedLocales ??= GetSupportedLocales(_providers.Values);

		public int? DefaultLocale { get; set; }

		public TypeLocalization GetTypeLocalization(Type type) => new(this, type);

		public string GetAssemblyString(Assembly assembly, string key) => GetString(GetAssemblyName(assembly), key, true);

		public string GetString(Type type, string key) => GetString(GetAssemblyName(type.Assembly), CombineKey(type.FullName!, key));

		public string GetString(string providerKey, string key, bool isAssemblyString = false)
		{
			if (isAssemblyString)
			{
				key = CombineKey(_assemblyPrefix, key);
			}

			return InternalGetString(providerKey, key);
		}

		internal static string CombineKey(string keyPrefix, string key) => String.Concat(keyPrefix, KeySeparator, key);

		private string InternalGetString(string providerKey, string key)
		{
			if (!_providers.TryGetValue(providerKey, out var provider))
			{
				throw new ArgumentException($"Provider with key {providerKey} is not registered", nameof(providerKey));
			}

			var culture = CurrentUICulture;

			do
			{
				var lcid = culture.LCID;

				string? result;

				if (provider.SupportedLocales.Contains(lcid))
				{
					result = provider.GetString(key, lcid);

					if (!String.IsNullOrEmpty(result))
					{
						return result;
					}
				}
				else if (_enableFallbackLocale && culture.Equals(CultureInfo.InvariantCulture))
				{
					result = DefaultLocale.HasValue ? provider.GetString(key, DefaultLocale.Value) : null;

					for (var i = 0; result == null && i < provider.SupportedLocales.Count; i++)
					{
						result = provider.GetString(key, provider.SupportedLocales[i]);
					}

					if (!String.IsNullOrEmpty(result))
					{
						return result;
					}
				}

				culture = culture.Equals(CultureInfo.InvariantCulture) ? null : culture.Parent;
			}
			while (culture != null);

			return GetMissingKey(providerKey, key, CurrentUICulture.LCID);
		}

		private static IReadOnlyList<int> GetSupportedLocales(IEnumerable<ILocalizationProvider> providers)
		{
			var hash = new HashSet<int>();

			foreach (var provider in providers)
			{
				foreach (var lcid in provider.SupportedLocales)
				{
					hash.Add(lcid);
				}
			}

			return hash.ToArray();
		}

		private string GetMissingKey(string providerKey, string key, int lcid)
		{
			if (_throwOnMissing)
			{
				throw new ArgumentException($"Localization string with key '{key}' was not found in provider with key '{providerKey}' for locale ID {lcid}");
			}

			return String.Format(_missingStringFormat, providerKey, key, lcid);
		}

		private static string GetAssemblyName(Assembly assembly)
		{
			return assembly.GetName().Name;
		}

		private static IReadOnlyDictionary<string, ILocalizationProvider> GetProviderKeys(IEnumerable<ILocalizationProvider> providers)
		{
			return new ReadOnlyDictionary<string, ILocalizationProvider>(providers.ToDictionary(prov => prov.Key));
		}
	}
}
