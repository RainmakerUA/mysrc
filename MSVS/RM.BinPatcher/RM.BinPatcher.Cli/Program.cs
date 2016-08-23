using System;
using System.IO;
using System.Linq;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Cli
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			//var pattern = Pattern.Parse("7E??????04 25 17 6A 58 80??????04 7E??????04 36??");
			//FindPattern(pattern);

			var patch = Patch.Parse(File.ReadAllLines(@"..\..\..\patch.fu.txt"));
			//ValidatePatch(patch);
			ApplyPatch(patch);

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

		private static void ValidatePatch(Patch patch)
		{
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Validating patch...");

			using (var stream = File.Open(@"e:\Work\RE\Notepad\notepad_3.bin", FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
			{
				using (var patcher = new Patcher(stream))
				{
					Console.WriteLine(patcher.Validate(patch) ? "Patch is valid" : "Patch is NOT valid");
				}
			}

			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Validation completed");
		}

		private static void ApplyPatch(Patch patch)
		{
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Applying patch...");

			using (var stream = File.Open(@"e:\Work\RE\Notepad\notepad_3.bin", FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
			{
				using (var bakStream = File.OpenWrite(@"e:\Work\RE\Notepad\notepad_3.bin.bak"))
				{
					stream.CopyTo(bakStream);
					stream.Position = 0;
				}

				using (var patcher = new Patcher(stream))
				{
					var result = patcher.Apply(patch);
					Console.WriteLine(result.IsSuccess ? "Patch is applied" : "Patch is NOT applied: " + result.Message);
				}
			}

			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Application completed");
		}
	}
}
