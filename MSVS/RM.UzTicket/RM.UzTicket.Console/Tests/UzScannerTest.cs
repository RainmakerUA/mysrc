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
								"VZH", "Іван", "Івашко", DateTime.Now.AddDays(7),
								Station.Create(2200001, "Kyiv"),
								Station.Create(2210700, "Dnipropetrovsk Holovny"),
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
