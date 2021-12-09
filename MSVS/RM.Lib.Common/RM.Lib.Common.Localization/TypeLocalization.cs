using System;

namespace RM.Lib.Common.Localization
{
	public sealed class TypeLocalization
	{
		private readonly LocalizationManager _manager;
		private readonly string _providerKey;
		private readonly string _typeKey;

		internal TypeLocalization(LocalizationManager manager, Type type)
		{
			_manager = manager;
			_providerKey = type.Assembly.GetName().Name;
			_typeKey = type.FullName!;
		}

		public string GetString(string key) => _manager.GetString(_providerKey, LocalizationManager.CombineKey(_typeKey, key));

		public string GetAssemblyString(string key) => _manager.GetString(_providerKey, key, true);
	}
}
