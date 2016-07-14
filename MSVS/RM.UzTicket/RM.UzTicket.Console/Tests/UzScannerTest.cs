using System;
using System.Threading;
using System.Threading.Tasks;
using RM.UzTicket.Lib;
using RM.UzTicket.Lib.Model;
using Con = System.Console;

namespace RM.UzTicket.Console.Tests
{
	internal static class UzScannerTest
	{
		private static AutoResetEvent _lock;

		public static void Run()
		{
			_lock = new AutoResetEvent(false);

			using (var scanner = new UzScanner(ScanCallback, 120))
			{
				scanner.AddItem(new ScanItem(
								"VZH", "Ivan", "Ivanov", DateTime.Now,//.AddDays(7),
								new Station { Id = 2200001, Title = "Kyiv" },
								new Station { Id = 2210700, Title = "Dnipropetrovsk Holovny" },
								"080К", "П"
							));
				_lock.WaitOne();
				_lock = null;
			}
		}

		private static Task ScanCallback(string callbackId, string sessionId)
		{
			Con.WriteLine($"Successful booking for {callbackId}: document.cookie='{sessionId}'");
			_lock?.Set();
			return Task.Delay(0);
		}
	}
}
