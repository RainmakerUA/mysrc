using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RM.UzTicket.Lib.Model
{/*
	public static class ModelExtensions
	{
		private static readonly Regex _toSnakeCase = new Regex("([a-z])([A-Z])");
		private static readonly IDictionary<Type, PropertyInfo[]> _propCache = new Dictionary<Type, PropertyInfo[]>();

		public static IDictionary<string, object> ToDictionary(this ModelBase model)
		{
			return GetProperties(model.GetType()).ToDictionary(GetPropertyKey, p => p.GetValue(model));
		}

		public static T ToModel<T>(this IDictionary<string, object> dict) where T : ModelBase, new()
		{
			var obj = new T();

			obj.LoadFromDictionary(dict);

			return obj;
		}

		public static T ToModel<T>(this IDictionary<string, JsonValue> dict) where T : ModelBase, new()
		{
			var obj = new T();

			obj.LoadFromDictionary(dict);

			return obj;
		}

		public static void LoadFromDictionary<T>(this ModelBase model, IDictionary<string, T> dict, bool throwUncastable = false)
		{
			foreach (var property in GetProperties(model.GetType()))
			{
				var key = GetPropertyKey(property);

				if (dict.ContainsKey(key))
				{
					AssignProperty(model, property, dict[key], throwUncastable);
				}
			}
		}

		public static bool EqualsTo(this IDictionary<string, object> first, IDictionary<string, object> second)
		{
			if (first == second) // reference equality
			{
				return true;
			}

			if (first == null)
			{
				return second == null;
			}

			if (second == null)
			{
				return false;
			}

			if (first.Keys.Count != second.Keys.Count)
			{
				return false;
			}

			var keys = new HashSet<string>(first.Keys.Concat(second.Keys));

			if (keys.Count != first.Keys.Count || keys.Count != second.Keys.Count)
			{
				return false;
			}

			foreach (var key in keys)
			{
				if (!Equals(first[key], second[key]))
				{
					return false;
				}
			}

			return true;
		}

		private static void AssignProperty(ModelBase model, PropertyInfo prop, object value, bool throwUncastable)
		{
			if (value != null)
			{
				var jsonValue = value as JsonValue;
				object val;

				var propType = prop.PropertyType;

				if (jsonValue != null && jsonValue.TryReadAs(propType, out val))
				{
					value = val;
				}

				var valType = value.GetType();

				if (!propType.GetTypeInfo().IsAssignableFrom(valType.GetTypeInfo()))
				{
					if (throwUncastable)
					{
						throw new InvalidCastException(
											$"Cannot assign value with type '{propType.FullName}' to property with type '{valType.FullName}'"
										);
					}
				}
				else
				{
					prop.SetValue(model, value);
				}
			}
		}

		private static string GetPropertyKey(PropertyInfo prop)
		{
			var name = prop.GetCustomAttribute<ModelPropertyAttribute>()?.Name;

			return String.IsNullOrEmpty(name)
					? _toSnakeCase.Replace(prop.Name, m => m.Groups[1].Value + '_' + m.Groups[2].Value).ToLowerInvariant()
					: prop.GetCustomAttribute<ModelPropertyAttribute>().Name;
		}

		private static PropertyInfo[] GetProperties(Type type)
		{
			if (!_propCache.ContainsKey(type))
			{
				_propCache[type] = type.GetRuntimeProperties().Where(p => p.CanRead && p.CanWrite).ToArray();
			}

			return _propCache[type];
		}
	}
*/}
