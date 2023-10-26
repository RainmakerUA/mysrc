using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace RM.Lib.Wpf.Common.MarkupExtensions
{
	public abstract class ManagedExtension : MarkupExtension
	{
		private const int _cleanupInterval = 10;

		private static readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

		private static readonly ConcurrentDictionary<Type, List<ManagedExtension>> _extensionRegistry = new ();
		private static readonly Func<Type, ManagedExtension, List<ManagedExtension>> _getExtensionListFunc = GetExtensionList;

		private static bool? _designerMode;
		private static int _cleanupCount;
		private static int _generationZeroCollectionCount;

		private readonly Type _type;
		private readonly List<WeakReference> _targets;

		protected ManagedExtension()
		{
			_type = GetType();
			_targets = new List<WeakReference>();

			RegisterInstance(this);
		}
		
		protected bool IsActive => _targets.Count == 0 || _targets.Any(target => target.IsAlive);

		protected IServiceProvider? ServiceProvider { get; private set; }

		protected object? TargetProperty { get; private set; }

		protected Type? TargetPropertyType => TargetProperty switch
												{
													DependencyProperty depProperty => depProperty.PropertyType,
													PropertyInfo propertyInfo => propertyInfo.PropertyType,
													_ => TargetProperty?.GetType()
												};

		protected static bool? DesignerMode => _designerMode;

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
			
			var provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();
			var target = provideValueTarget?.TargetObject;

			if (target != null && target.GetType().FullName != "System.Windows.SharedDp")
			{
				if (!_designerMode.HasValue && target is DependencyObject depTarget)
				{
					_designerMode = depTarget.IsDesignerMode();
				}

				_targets.Add(new WeakReference(target));

				TargetProperty = provideValueTarget?.TargetProperty;

				return GetValue(target) ?? this;
			}

			return this;
		}

		protected abstract object? GetValue(object target);

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

		protected static void UpdateTargets(Type type)
		{
			if (_extensionRegistry.TryGetValue(type, out var list))
			{
				foreach (var extension in list)
				{
					extension.UpdateTargets();
				}
			}
		}

		private void UpdateTarget(object target)
		{
			if (TargetProperty is DependencyProperty depProperty && target is DependencyObject depObj)
			{
				RunInDispatcher(SetDependencyProperty, true, DispatcherPriority.DataBind, depObj, depProperty, GetValue(target));
			}
			else if (TargetProperty is PropertyInfo propertyInfo)
			{
				RunInDispatcher(SetProperty, true, DispatcherPriority.DataBind, target, propertyInfo, GetValue(target));
			}
		}

		private static void SetDependencyProperty(DependencyObject obj, DependencyProperty property, object? value) => obj.SetValue(property, value);

		private static void SetProperty(object obj, PropertyInfo property, object? value) => property.SetValue(obj, value);

		private static void RegisterInstance(ManagedExtension extension)
		{
			_extensionRegistry.GetOrAdd(extension._type, _getExtensionListFunc, extension).Add(extension);

			var collectionCount = GC.CollectionCount(0);
			
			if (++_cleanupCount >= _cleanupInterval && collectionCount > _generationZeroCollectionCount)
			{
				_extensionRegistry[extension._type] = _extensionRegistry[extension._type].Where(ex => ex.IsActive).ToList();
				_cleanupCount = 0;
				_generationZeroCollectionCount = collectionCount;
			}
		}

		private static List<ManagedExtension> GetExtensionList(Type _, ManagedExtension ext) => new () { ext };
		
		private static void RunInDispatcher<T1, T2, T3>(Action<T1, T2, T3> action, bool async, DispatcherPriority priority, T1 arg1, T2 arg2, T3 arg3)
		{
			if (async)
			{
				_dispatcher.BeginInvoke(action, priority, arg1, arg2, arg3);
			}
			else
			{
				_dispatcher.Invoke(action, priority, arg1, arg2, arg3);
			}
		}
	}
}
