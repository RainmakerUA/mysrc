using System;

namespace RM.BinPatcher.Exceptions
{
	public class PatternParseException : Exception
	{
		internal PatternParseException(string pattern, int position)
				: base($"Error in pattern format at position {position}")
		{
			Pattern = pattern;
			Position = position;
		}

		public string Pattern { get; }

		public int Position { get; }
	}
}
