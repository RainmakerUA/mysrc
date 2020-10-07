using System;

namespace RM.Lib.Wpf.Common.MarkupExtensions
{
	public static class Helper
	{
		public static T GetService<T>(this IServiceProvider provider) where T : class => provider.GetService(typeof(T)) as T;
	}
}
