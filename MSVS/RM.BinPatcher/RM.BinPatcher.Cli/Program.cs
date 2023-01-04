using System.IO.MemoryMappedFiles;
using RM.BinPatcher.Enumerators;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Cli
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var filename = @"e:\Downloads\World of Warcraft Chronicle Volume III (2018).cbr";
			var pattern = Pattern.Parse(/*"F???2DE9"*/"1?2?3?4?5?6?7?8?");
			FindPattern(pattern, filename);
			//FindPattern2(pattern, filename);
			FindPattern3(pattern, filename);
			
			//var patch = Patch.Parse(File.ReadAllLines(@"..\..\..\patch.fu.txt"));
			//ValidatePatch(patch);
			//ApplyPatch(patch);

			Console.WriteLine("Press any key to exit");
			Console.ReadKey(true);
		}

		private static void FindPattern(Pattern pattern, string filename)
		{
			Console.WriteLine($"Using file {filename}");
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [MEMSPAN] Searching pattern matches...");
			
			var arr = File.ReadAllBytes(filename);
			var count = 0;

			foreach (var pos in new PatternEnumeratorSpan(arr, pattern))
			{
				Console.Write($"\rPattern found @ 0x{pos:X8}. Total: {++count:###}");
			}

			Console.WriteLine();
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Search completed");
		}

		// Too sloooooow
		//private static void FindPattern0(Pattern pattern, string filename)
		//{
		//	Console.WriteLine($"Using file {filename}");
		//	Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [MEMARR] Searching pattern matches...");
			
		//	var arr = File.ReadAllBytes(filename);
		//	var count = 0;

		//	foreach (var pos in new PatternEnumeratorArray(arr, pattern))
		//	{
		//		Console.Write($"\rPattern found @ 0x{pos:X8}. Total: {++count:###}");
		//	}

		//	Console.WriteLine();
		//	Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Search completed");
		//}

		private static void FindPattern2(Pattern pattern, string filename)
		{
			Console.WriteLine($"Using file {filename}");
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [MM] Searching pattern matches...");

			//using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			//{
			//	using (var patcher = new Patcher(stream))
			//	{
			//		foreach (var offset in patcher.FindPattern(pattern))
			//		{
			//			Console.WriteLine($"Pattern found @0x{offset:X8}");
			//		}
			//	}
			//}

			using (var mmap = MemoryMappedFile.CreateFromFile(filename, FileMode.Open))
			{
				using (var mmStr = mmap.CreateViewStream())
				{
					var count = 0;

					foreach (var pos in new PatternEnumerator(mmStr, pattern))
					{
						Console.Write($"\rPattern found @ 0x{pos:X8}. Total: {++count:###}");
					}
				}
			}

			Console.WriteLine();
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Search completed");
		}

		private static void FindPattern3(Pattern pattern, string filename)
		{
			Console.WriteLine($"Using file {filename}");
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [FS] Searching pattern matches...");

			using (var mmStr = File.OpenRead(filename))
			{
				var count = 0;

				foreach (var pos in new PatternEnumerator(mmStr, pattern))
				{
					Console.Write($"\rPattern found @ 0x{pos:X8}. Total: {++count:###}");
				}
			}

			Console.WriteLine();
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
