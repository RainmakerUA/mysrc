
using RM.BinPatcher.Parsers;

namespace RM.BinPatcher.Model
{
	public class Pattern
	{
		internal Pattern(byte?[] bytes)
		{
			Bytes = bytes;
		}

		public byte? this[int index] => Bytes[index];

		public int Length => Bytes.Length;

		internal byte?[] Bytes { get; }

		public static Pattern Parse(string pattern)
		{
			return new Pattern(PatternParser.ParseBytes(pattern));
		}
	}
}
