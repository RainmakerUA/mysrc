using System;
using System.Collections.Generic;
using System.Globalization;
using RM.BinPatcher.Exceptions;

namespace RM.BinPatcher.Parsers
{
	internal static class PatternParser
	{
		private const char _wildcard = '?';
		private const char _0 = '0';
		private const char _9 = '9';
		private const char _a = 'a';
		private const char _f = 'f';

		public static byte?[] ParseBytes(string pattern)
		{
			var lowPattern = pattern.ToLowerInvariant();
			var bytes = new List<byte?>();
			var firstChar = pattern[0];
			var index = 1;

			while (index < lowPattern.Length)
			{
				var secondChar = lowPattern[index];
			
				if (Char.IsWhiteSpace(firstChar))
				{
					firstChar = secondChar;
					index += 1;
					continue;
				}
				
				if (firstChar == _wildcard && secondChar == _wildcard)
				{
					bytes.Add(null);
					firstChar = SafeGetChar(lowPattern, index + 1);
					index += 2;
					continue;
				}

				if ((CharInRange(firstChar, _0, _9) || CharInRange(firstChar, _a, _f))
				    && (CharInRange(secondChar, _0, _9) || CharInRange(secondChar, _a, _f)))
				{
					var hex = new String(new[] {firstChar, secondChar});
					var value = Byte.Parse(hex, NumberStyles.AllowHexSpecifier);

					bytes.Add(value);
					firstChar = SafeGetChar(lowPattern, index + 1);
					index += 2;
					continue;
				}

				throw new PatternParseException(pattern, index);
			}

			return bytes.ToArray();
		}

		private static char SafeGetChar(string str, int index)
		{
			return index < str.Length ? str[index] : '\0';
		}

		private static bool CharInRange(char ch, char low, char high)
		{
			return low <= ch && ch <= high;
		}
	}
}