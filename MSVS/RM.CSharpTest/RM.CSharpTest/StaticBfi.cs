using System;

namespace RM.CSharpTest
{
	internal class StaticBfi
	{
		private static readonly string _stat;

		public static object Stat => _stat;

		static StaticBfi()
		{
			//Console.WriteLine("StaticBfi..cctor()");
			_stat = Echo("StaticBfi init");
		}

		public static string Echo(string text)
		{
			Console.WriteLine("StaticBfi says: " + text);
			return text;
		}
	}
}
