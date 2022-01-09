using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace RM.Lib.Wpf.Common
{
	public static class Extensions
	{
		public static T? GetService<T>(this IServiceProvider provider) where T : class => provider.GetService(typeof(T)) as T;

		public static bool IsDesignerMode(this DependencyObject depObj) => DesignerProperties.GetIsInDesignMode(depObj);

		public static bool IsDesignerMode(this IServiceProvider provider) =>
				provider.GetService<IProvideValueTarget>()?.TargetObject is DependencyObject target && target.IsDesignerMode();

		public static DependencyObject GetVisualRoot(this DependencyObject depObject)
		{
			while (VisualTreeHelper.GetParent(depObject) is { } parent)
			{
				depObject = parent;
			}

			return depObject;
		}
	}
}
