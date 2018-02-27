using System;

namespace RM.CSharpTest
{
	internal class Static
	{
		private static readonly string _stat = Echo("Static init");

		public static object Stat => _stat;

		//static Static()
		//{
		//	Console.WriteLine("Static..cctor()");
		//}

		public static string Echo(string text)
		{
			Console.WriteLine("Static says: " + text);
			return text;
		}
	}
}
