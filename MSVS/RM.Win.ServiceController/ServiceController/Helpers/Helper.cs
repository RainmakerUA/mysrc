using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ServiceController.Helpers
{
	internal static class Helper
	{
		private static readonly Regex _enumRegex = new Regex("([a-z])([A-Z])");

		public static string GetEnumText(Enum e)
		{
			MatchEvaluator meval = m => String.Format(
												"{0} {1}",
			                            		m.Groups[1].Value,
			                            		m.Groups[2].Value.ToLowerInvariant()
			                            	);
			var valueName = Enum.GetName(e.GetType(), e);
			return _enumRegex.Replace(valueName, meval);
		}

		public static string GetAssemblyVersion()
		{
			var ver = Assembly.GetEntryAssembly().GetName().Version;
			return ver == null ? null : ver.ToString();
		}
	}
}
