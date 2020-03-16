using System;
using System.Linq;

namespace RM.CSharpTest.Sharp8
{
	internal static class Sharp8Test
	{
		public static void Execute()
		{
			var test = ITest1.Create(Enum.GetValues(typeof(TypeCode)).Cast<TypeCode>().Select(v => v.ToString()));
			var testArr = test.ToArray();
			PrintArray("testArray", testArr);

			if (test is ITest0<string> test0)
			{
				PrintArray("test0 array", test0.ToArray());
			}
		}

		private static void PrintArray<T>(string title, T[] array)
		{
			Console.WriteLine(title);
			foreach (var item in array)
			{
				Console.WriteLine("\t{0}", item);
			}
			Console.WriteLine();
		}
	}
}
