using System;
using System.IO;
using System.IO.Compression;

namespace RM.Util.GZip
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			if (args.Length < 3)
			{
				ShowUsageAndExit();
			}

			var isUnpackMode = false;

			switch (args[0].ToLowerInvariant())
			{
				case "u":
				case "unpack":
					isUnpackMode = true;
					break;

				case "p":
				case "pack":
					isUnpackMode = false;
					break;

				default:
					ShowUsageAndExit();
					break;
			}

			try
			{
				if (isUnpackMode)
				{
					Unpack(args[1], args[2]);
				}
				else
				{
					Pack(args[1], args[2]);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e.GetType().Name} :: {e.Message}");
			}
		}

		private static void Pack(string inFile, string outFile)
		{
			using (var ifs = File.OpenRead(inFile))
			{
				using (var ofs = File.OpenWrite(outFile))
				{
					using (var gzs = new GZipStream(ofs, CompressionMode.Compress, true))
					{
						ifs.CopyTo(gzs);
					}
				}
			}
		}

		private static void Unpack(string inFile, string outFile)
		{
			using (var ifs = File.OpenRead(inFile))
			{
				using (var ofs = File.OpenWrite(outFile))
				{
					using (var gzs = new GZipStream(ifs, CompressionMode.Decompress, true))
					{
						gzs.CopyTo(ofs);
					}
				}
			}
		}

		private static void ShowUsageAndExit()
		{
			Console.WriteLine($"Usage: {Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location)} p(ack)|u(npack) in-file out-file");
			Environment.Exit(-19);
		}
	}
}
