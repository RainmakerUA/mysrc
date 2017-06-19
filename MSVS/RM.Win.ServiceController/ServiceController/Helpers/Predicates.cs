using System;

namespace ServiceController.Helpers
{
	internal static class Predicates
	{
		public static Predicate<T> Not<T>(Predicate<T> pred)
		{
			return delegate(T arg)
					{
						return !pred(arg);
					};
		}

		public static Predicate<T> And<T>(Predicate<T> pred1, Predicate<T> pred2)
		{
			return delegate(T arg)
					{
						return pred1(arg) && pred2(arg);
					};
		}

		public static Predicate<T> Or<T>(Predicate<T> pred1, Predicate<T> pred2)
		{
			return delegate(T arg)
					{
						return pred1(arg) || pred2(arg);
					};
		}
	}
}
