using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace RM.Lib.Wpf.Common.MarkupExtensions
{
	public abstract class ManagedMarkupExtension : MarkupExtension
	{
		private const int _cleanupInterval = 10;
		private static readonly Dispatcher _dispatcher = Application.Current.Dispatcher;
		private static readonly Dictionary<string, List<ManagedMarkupExtension>> _extensionRegistry = new Dictionary<string, List<ManagedMarkupExtension>>();

		private static int _cleanupCount = 0;

		private readonly List<WeakReference> _targets;
		private bool? _isDesignerMode;

		protected ManagedMarkupExtension()
		{
			_targets = new List<WeakReference>();

			RegisterInstance(this);
		}

		protected bool IsActive => _targets.Count == 0 || _targets.Any(target => target.IsAlive);

		protected IServiceProvider ServiceProvider { get; private set; }

		protected object TargetProperty { get; private set; }

		protected Type TargetPropertyType
		{
			get
			{
				switch (TargetProperty)
				{
					case DependencyProperty depProperty:
						return depProperty.PropertyType;

					case PropertyInfo propertyInfo:
						return propertyInfo.PropertyType;

					default:
						return TargetProperty?.GetType();
				}
			}
		}

		protected bool IsDesignerMode
		{
			get
			{
				if (!_isDesignerMode.HasValue)
				{
					var targetProvider = ServiceProvider.GetService<IProvideValueTarget>();
					_isDesignerMode = !(targetProvider.TargetObject is DependencyObject target) || DesignerProperties.GetIsInDesignMode(target);
				}

				return _isDesignerMode.Value;
			}
		}

		//protected IReadOnlyList<WeakReference<DependencyObject>> Targets => _targets.AsReadOnly();

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
			RegisterTarget(serviceProvider);

			return TargetProperty != null ? GetValue() : this;
		}

		private void RegisterTarget(IServiceProvider serviceProvider)
		{
			var provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();
			var target = provideValueTarget?.TargetObject;

			if (target != null && target.GetType().FullName != "System.Windows.SharedDp")
			{
				TargetProperty = provideValueTarget.TargetProperty;
				_targets.Add(new WeakReference(target));
			}
		}
		private void UpdateTarget(object target)
		{
			if (TargetProperty is DependencyProperty depProperty && target is DependencyObject depObj)
			{
				_dispatcher.BeginInvoke(new Action<DependencyObject, DependencyProperty, object>(SetDependencyProperty), DispatcherPriority.DataBind, depObj, depProperty, GetValue());
			}
			else if (TargetProperty is PropertyInfo propertyInfo)
			{
				_dispatcher.BeginInvoke(new Action<object, PropertyInfo, object>(SetProperty), DispatcherPriority.DataBind, target, propertyInfo, GetValue());
			}
		}

		protected void UpdateTargets()
		{
			foreach (var weakRef in _targets)
			{
				var target = weakRef.Target;

				if (target != null)
				{
					UpdateTarget(target);
				}
			}
		}

		protected abstract object GetValue();

		private static void SetDependencyProperty(DependencyObject obj, DependencyProperty property, object value) => obj.SetValue(property, value);

		private static void SetProperty(object obj, PropertyInfo property, object value) => property.SetValue(obj, value);

		private static void RegisterInstance(ManagedMarkupExtension extension)
		{
			var key = extension.GetType().FullName;

			_dispatcher.Invoke(new Action<string, ManagedMarkupExtension>(RegisterInstanceInternal), DispatcherPriority.Normal, key, extension);
			
			if (++_cleanupCount >= _cleanupInterval)
			{
				_cleanupCount = 0;
				_dispatcher.Invoke(new Action<string>(CleanupInternal), DispatcherPriority.Background, key);
			}

		}

		private static void RegisterInstanceInternal(string key, ManagedMarkupExtension extension)
		{
			if (!_extensionRegistry.TryGetValue(key, out var list))
			{
				list = new List<ManagedMarkupExtension>();
				_extensionRegistry.Add(key, list);
			}

			list.Add(extension);
		}

		private static void CleanupInternal(string key)
		{
			_extensionRegistry[key] = _extensionRegistry[key].Where(ex => ex.IsActive).ToList();
		}
	}
}
