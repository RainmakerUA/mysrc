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

		private readonly string _key;

		private LocalizationManager? _localization;
		private Type? _resourceType;

		public LocalizationExtension(Type? resourceType, string key)
		{
			_resourceType = resourceType;
			_key = key;
		}

		public LocalizationExtension(string key) : this(null, key)
		{
		}

		public bool IsAssemblyResource { get; init; }

		private LocalizationManager Localization => _localization ??= LocalizationManager.Instance;

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
					? Localization.GetAssemblyString(_resourceType.Assembly, _key)
					: Localization.GetString(_resourceType, _key);
		}

		public static void Update()
		{
			UpdateTargets(_type);
		}
	}
}
