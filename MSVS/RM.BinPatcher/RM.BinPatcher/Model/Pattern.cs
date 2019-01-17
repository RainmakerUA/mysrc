using System;
using RM.BinPatcher.Parsers;

namespace RM.BinPatcher.Model
{
	public class Pattern
	{
		internal Pattern(BytePart[] bytes)
		{
			Bytes = bytes;
		}

		public BytePart this[int index] => Bytes[index];

		public int Length => Bytes.Length;

		internal BytePart[] Bytes { get; }

		public override string ToString()
		{
			return Bytes == null ? "[Empty]" : String.Join("\u0020", Bytes);
		}

		public static Pattern Parse(string pattern)
		{
			return new Pattern(PatternParser.ParseBytes(pattern));
		}
	}
}
