using System;
using System.Collections.Generic;
using System.IO;
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
			
			var locker = new Lib.Utils.AsyncLock(_locker);

			Con.Write("Press any key to exit:");
			Con.ReadKey(true);
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
				await Tests.UzClientTest.Run();
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
			var list = new List<string> {"test", "test: again", "one-more: test"};
			File.WriteAllText(@"D:\out_n.txt", String.Join("\n", list));
			File.WriteAllText(@"D:\out_rn.txt", String.Join("\r\n", list));
			File.WriteAllText(@"D:\out_newline.txt", String.Join(Environment.NewLine, list));
		}
	}
}
