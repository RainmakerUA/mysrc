using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RM.Win.ServiceController.Common
{
    internal static class Extensions
    {
		private static readonly Regex _enumRegex = new("([a-z])([A-Z])", RegexOptions.Compiled);

		public static bool IsDefault(this double value)
		{
			return Double.IsNaN(value) || Math.Abs(value) < Double.Epsilon;
		}

		public static string ToDisplayText<T>(this T e) where T : Enum
        {
			var valueName = Enum.GetName(e.GetType(), e);
			return String.IsNullOrEmpty(valueName) ? String.Empty : _enumRegex.Replace(valueName, Evaluator);

			static string Evaluator(Match m) => $"{m.Groups[1].Value} {m.Groups[2].Value.ToLowerInvariant()}";
		}

		public static void Catch(this Task task, Action<Exception?>? handler)
		{
			task.ContinueWith(
								t =>
									{
										if (t.IsFaulted && t.Exception != null)
										{
											handler?.Invoke(t.Exception?.GetBaseException());
										}
									}
							);
		}

		public static Exception? GetInnerException(this AggregateException aggrExc) => aggrExc.Flatten().InnerException;
	}
}
