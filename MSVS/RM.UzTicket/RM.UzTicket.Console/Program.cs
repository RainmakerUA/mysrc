using System;
using System.Threading;
using Con = System.Console;

namespace RM.UzTicket.Console
{
	internal static class Program
	{
		private static readonly AutoResetEvent _locker = new AutoResetEvent(true);

		private static void Main(string[] args)
		{
			RunTests();
			
			IDisposable locker = new Lib.Utils.AsyncLock(_locker);

			Con.Write("Press any key to exit:");
			Con.ReadKey(true);

			locker.Dispose();
		}

		private static async void RunTests()
		{
			Con.WriteLine("Running tests...");

			IDisposable locker = new Lib.Utils.AsyncLock(_locker);

			try
			{
				InlineTest();
				Tests.JjDecoderTest.Run();
				Tests.UtilsTest.TestParseToken();
				Tests.ModelTest.Run();
				//await Tests.UzClientTest.Run();
				Tests.UzScannerTest.Run();
				// ...

				Con.WriteLine("Tests completed!");
			}
			catch (Exception e)
			{
				Con.WriteLine($"Test failed:\n{e}");
			}
			finally
			{
				locker.Dispose();
			}
		}

		private static void InlineTest()
		{
			
		}
	}
}
