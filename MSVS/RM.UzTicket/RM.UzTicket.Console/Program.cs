using System;
using Con = System.Console;

namespace RM.UzTicket.Console
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			RunTests();
			
			Con.Write("Press any key to exit:");
			Con.ReadKey(true);
		}

		private static void RunTests()
		{
			Con.WriteLine("Running tests...");

			try
			{
				Tests.JjDecoderTest.Run();
				Tests.UtilsTest.TestParseToken();
				Tests.ModelTest.Run();
				// ...

				Con.WriteLine("Tests completed!");
			}
			catch (Exception e)
			{
				Con.WriteLine($"Test failed:\n{e}");
			}
		}
	}
}
