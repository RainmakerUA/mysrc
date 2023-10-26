using System;
using System.IO;

namespace RM.Lib.Common
{
	public static class ProcessLocation
	{
		static ProcessLocation()
		{
			var exePath = Environment.GetCommandLineArgs() is { Length: > 0 } cmdLineArgs ? cmdLineArgs[0] : null;

			if (String.IsNullOrEmpty(exePath))
			{
				exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
			}

			if (!String.IsNullOrEmpty(exePath))
			{
				if (!Path.IsPathRooted(exePath))
				{
					exePath = Path.GetFullPath(exePath);
				}

				ProcessExePath = Path.GetDirectoryName(exePath);
				ProcessExeName = Path.GetFileNameWithoutExtension(exePath);
				ProcessExeLocation = exePath;
			}
			else
			{
				ProcessExePath = AppContext.BaseDirectory;
			}
		}

		public static string? ProcessExePath { get; }

		public static string? ProcessExeName { get; }

		public static string? ProcessExeLocation { get; }
	}
}
