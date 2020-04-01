using System;
using System.Linq.Expressions;
using System.Reflection;

namespace RM.Win.Utils.Flasher
{
    public static class Extensions
    {
        /// <summary>
        ///     Extracts the property name from a property expression.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpression">The property expression (e.g. p => p.PropertyName)</param>
        /// <returns>The name of the property.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="propertyExpression" /> is null.</exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when the expression is:<br />
        ///     Not a <see cref="MemberExpression" /><br />
        ///     The <see cref="MemberExpression" /> does not represent a property.<br />
        ///     Or, the property is static.
        /// </exception>
        public static string GetName<T>(this Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (propertyExpression.Body is MemberExpression memberExpression
					&& memberExpression.Member is PropertyInfo property
					&& property.GetMethod != null && !property.GetMethod.IsStatic)
            {
				return memberExpression.Member.Name;
			}

            throw new ArgumentException("Incorrect expression type", nameof(propertyExpression));
        }
    }
}
