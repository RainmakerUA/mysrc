using System;
using System.Collections.Generic;
using System.Reflection;

namespace RM.WP.GpsMonitor.Common
{
	public sealed class UnitEnumHelper<T> where T : struct
	{
		private const string _keySeparator = "_";
		private const string _descriptionSuffix = "_L";

		private readonly Type _type;
		private readonly string _resPrefix;
		private readonly IDictionary<T, UnitEnumValueAttribute> _entries;
		private readonly IDictionary<T, UnitEnumValueAttribute> _entriesWithText;

		public UnitEnumHelper()
		{
			var enumType = typeof(T);

			_type = enumType;

			var typeInfo = enumType.GetTypeInfo();

			if (!typeInfo.IsEnum)
			{
				throw new ArgumentException("Type passed is not Enum type!", nameof(T));
			}

			var unitEnumAttr = typeInfo.GetCustomAttribute<UnitEnumAttribute>();

			if (unitEnumAttr == null)
			{
				throw new ArgumentException("Type passed does not have UnitEnumAttribute!", nameof(T));
			}

			_resPrefix = unitEnumAttr.ResourcePrefix ?? _type.Name;

			var entries = GetEntriesWithKeys();
			_entries = entries;
			_entriesWithText = GetEntriesWithText(entries);
		}

		public IEnumerable<UnitEnumValueAttribute> Entries => _entries.Values;

		public IEnumerable<UnitEnumValueAttribute> EntriesWithText => _entriesWithText.Values;

		public UnitEnumValueAttribute GetEntry(T key) => _entries.ContainsKey(key) ? _entries[key] : null;

		public UnitEnumValueAttribute GetEntryWithText(T key) => _entriesWithText.ContainsKey(key) ? _entriesWithText[key] : null;

		private IDictionary<T, UnitEnumValueAttribute> GetEntriesWithKeys()
		{
			var entries = Enum.GetValues(_type);
			var result = new Dictionary<T, UnitEnumValueAttribute>();

			foreach (T entry in entries)
			{
				var fieldInfo = _type.GetRuntimeField(entry.ToString());
				var attr = fieldInfo.GetCustomAttribute<UnitEnumValueAttribute>();
				if (attr != null)
				{
					var key = attr.NameKey ?? entry.ToString();
					var descr = attr.DescriptionKey ?? key + _descriptionSuffix;
					attr.NameKey = $"{_resPrefix}{_keySeparator}{key}";
					attr.DescriptionKey = $"{_resPrefix}{_keySeparator}{descr}";

					result.Add(entry, attr);
				}
			}

			return result;
		}

		private static IDictionary<T, UnitEnumValueAttribute> GetEntriesWithText(IDictionary<T, UnitEnumValueAttribute> entriesWithKeys)
		{
			var result = new Dictionary<T, UnitEnumValueAttribute>();

			foreach (var key in entriesWithKeys.Keys)
			{
				var entry = entriesWithKeys[key];
				var nameKey = ResourceHelper.GetString(entry.NameKey);
				var descriptionKey = ResourceHelper.GetString(entry.DescriptionKey);

				result.Add(key, new UnitEnumValueAttribute(nameKey) { Coefficient = entry.Coefficient, DescriptionKey = descriptionKey });
			}

			return result;
		}
	}
}
