using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Cli
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var pattern = Pattern.Parse("7E??????04 25 17 6A 58 80??????04 7E??????04 36??");
			var tokenSource = new CancellationTokenSource(10000);

			//FindPattern(pattern);
			//FindPatternAsync(pattern, tokenSource.Token);
			FindPatternNoAwait(pattern, /*tokenSource.Token*/CancellationToken.None);

			Console.WriteLine("Press any key to exit");
			Console.ReadKey(true);
		}

		private static void FindPattern(Pattern pattern)
		{
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Searching pattern matches...");

			using (var stream = File.Open(@"e:\Work\RE\Notepad\notepad_3.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var patcher = new Patcher(stream))
				{
					foreach (var offset in patcher.FindPattern(pattern))
					{
						Console.WriteLine($"Pattern found @0x{offset:X8}");
					}
				}
			}

			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Search completed");
		}

		private static async void FindPatternAsync(Pattern pattern, CancellationToken token)
		{
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Searching pattern matches...");

			using (var stream = File.Open(@"e:\Work\RE\Notepad\notepad_3.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var patcher = new Patcher(stream))
				{
					foreach (var offsetTask in await patcher.FindPatternAsync(pattern, token))
					{
						var offset = await offsetTask;
						Console.WriteLine($"Pattern found @0x{offset:X8}");
					}
				}
			}

			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Search completed");
		}

		private static async void FindPatternNoAwait(Pattern pattern, CancellationToken token)
		{
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Searching pattern matches...");

			using (var stream = File.Open(@"e:\Work\RE\Notepad\notepad_3.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var patcher = new Patcher(stream))
				{
					var tasks = new List<Task<long>>();

					foreach (var offsetTask in await patcher.FindPatternAsync(pattern, token))
					{
						tasks.Add(offsetTask);
					}

					foreach (var offset in await Task.WhenAll(tasks))
					{
						Console.WriteLine($"Pattern found @0x{offset:X8}");
					}
				}
			}

			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Search completed");
		}
	}
}
