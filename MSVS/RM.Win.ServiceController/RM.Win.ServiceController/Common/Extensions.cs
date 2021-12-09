using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RM.Win.ServiceController.Common
{
    internal static class Extensions
    {
		private static readonly Regex _enumRegex = new("([a-z])([A-Z])", RegexOptions.Compiled);

		/// <summary>
        /// Extracts the property name from a property expression.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpression">The property expression (e.g. p => p.PropertyName)</param>
        /// <param name="propExpr">Filled by compiler text representation of an expression passed to <paramref name="propertyExpression"/></param>
        /// <returns>The name of the property.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="propertyExpression" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the expression is:<br />
        ///     Not a <see cref="MemberExpression" /><br />
        ///     The <see cref="MemberExpression" /> does not represent a property.<br />
        ///     Or, the property is static.
        /// </exception>
        public static string GetName<T>(this Expression<Func<T>> propertyExpression, [CallerArgumentExpression("propertyExpression")] string propExpr = null!)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (propertyExpression.Body is MemberExpression { Member: PropertyInfo property } memberExpression
					&& property.GetMethod != null && !property.GetMethod.IsStatic)
            {
				return memberExpression.Member.Name;
			}

            throw new ArgumentException($"Incorrect expression type of '{propExpr}'", nameof(propertyExpression));
		}

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

		public static void Catch(this Task task, Action<Exception>? handler)
		{
			task.ContinueWith(
								t =>
									{
										if (t.IsFaulted && t.Exception != null)
										{
											handler?.Invoke(t.Exception.GetBaseException());
										}
									}
							);
		}

		public static Exception GetInnerException(this AggregateException aggrExc) => aggrExc.Flatten().InnerException;
	}
}
