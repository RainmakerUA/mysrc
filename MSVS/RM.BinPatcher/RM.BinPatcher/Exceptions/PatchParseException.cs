using System;

namespace RM.BinPatcher.Exceptions
{
	public class PatchParseException: Exception
	{
		internal PatchParseException(string line, int lineNumber, Exception? innerException = null)
				: base($"Error in patch format at line {lineNumber}: {innerException?.Message ?? "Unspecified"}")
		{
			Line = line;
			LineNumber = lineNumber;
		}

		public string Line { get; }

		public int LineNumber { get; }
	}
}
