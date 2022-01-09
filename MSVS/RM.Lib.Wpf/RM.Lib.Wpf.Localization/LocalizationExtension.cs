using System.Windows;
using System.Xaml;
using RM.Lib.Common.Localization;
using RM.Lib.Wpf.Common;
using RM.Lib.Wpf.Common.MarkupExtensions;

namespace RM.Lib.Wpf.Localization
{
	public sealed class LocalizationExtension : ManagedExtension
	{
		private const string _designFormat = "LOC: {0}";
		private const string _designAsmFormat = "LOC: (ASM) {0}";

		private static readonly Type _type = typeof(LocalizationExtension);

		private readonly LocalizationManager _localization;
		private readonly string _key;

		private Type? _resourceType;

		public LocalizationExtension(Type? resourceType, string key)
		{
			_resourceType = resourceType;
			_key = key;
			_localization = LocalizationManager.Instance;
		}

		public LocalizationExtension(string key) : this(null, key)
		{
		}

		public bool IsAssemblyResource { get; init; }

		protected override object? GetValue(object target)
		{
			if (DesignerMode ?? false)
			{
				return String.Format(IsAssemblyResource ? _designAsmFormat : _designFormat, _key);
			}

			if (_resourceType is null)
			{
				var rootProvider = ServiceProvider?.GetService<IRootObjectProvider>();
				var rootObject = rootProvider?.RootObject
									?? (target is DependencyObject depTarget ? depTarget.GetVisualRoot() : null);

				_resourceType = rootObject?.GetType();
			}

			if (_resourceType is null)
			{
				return null;
			}

			return IsAssemblyResource
					? _localization.GetAssemblyString(_resourceType.Assembly, _key)
					: _localization.GetString(_resourceType, _key);
		}

		public static void Update()
		{
			UpdateTargets(_type);
		}
	}
}
