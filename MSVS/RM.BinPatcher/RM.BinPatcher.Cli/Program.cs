using System;
using System.IO;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Cli
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			//var pattern = Pattern.Parse("7E??????04 25 17 6A 58 80??????04 7E??????04 36??");
			//FindPattern(pattern);

			var patch = Patch.Parse(File.ReadAllLines(@"..\..\..\patch.features.txt"));

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
	}
}
