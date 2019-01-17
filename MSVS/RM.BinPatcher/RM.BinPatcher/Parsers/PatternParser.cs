using System;
using System.Collections.Generic;
using RM.BinPatcher.Exceptions;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Parsers
{
	internal static class PatternParser
	{
		private const char _wildcard = '?';
		private const char _0 = '0';
		private const char _9 = '9';
		private const char _a = 'a';
		private const char _f = 'f';

		public static BytePart[] ParseBytes(string pattern)
		{
			var bytes = new List<BytePart>();
			var firstChar = pattern[0];
			var index = 1;

			while (index < pattern.Length)
			{
				var secondChar = pattern[index];
			
				if (Char.IsWhiteSpace(firstChar))
				{
					firstChar = secondChar;
					index += 1;
					continue;
				}
				
				if (firstChar == _wildcard)
				{
					if (secondChar == _wildcard)
					{
						bytes.Add(BytePart.Any);
					}
					else if (CharIsHex(secondChar))
					{
						bytes.Add(BytePart.Low(ParseHex(secondChar)));
					}
					else
					{
						Throw();//(pattern, index);
					}
				}
				else if (CharIsHex(firstChar))
				{
					if (secondChar == _wildcard)
					{
						bytes.Add(BytePart.High(ParseHex(firstChar)));
					}
					else if (CharIsHex(secondChar))
					{
						bytes.Add(BytePart.Full(ParseHexes(firstChar, secondChar)));
					}
					else
					{
						Throw();//(pattern, index);
					}
				}
				else
				{
					Throw(); //(pattern, index);
				}

				firstChar = SafeGetChar(pattern, index + 1);
				index += 2;
			}

			return bytes.ToArray();

			bool CharInRange(char ch, char low, char high)
			{
				return low <= ch && ch <= high;
			}

			bool CharIsHex(char c)
			{
				return CharInRange(c, _0, _9) || CharInRange(Char.ToLowerInvariant(c), _a, _f);
			}

			char SafeGetChar(string str, int charIndex)
			{
				return charIndex < str.Length ? str[charIndex] : '\0';
			}

			byte ParseHex(char hex)
			{
				if (CharInRange(hex, _0, _9))
				{
					return (byte)(hex - 0x30);
				}

				var lower = Char.ToLowerInvariant(hex);

				if (CharInRange(lower, _a, _f))
				{
					return (byte)(lower - 0x61 + 0x0A);
				}

				Throw();
				return 0; // never returns
			}

			byte ParseHexes(char highHex, char lowHex) => (byte)(ParseHex(highHex) << 4 | ParseHex(lowHex));
			
			void Throw()//(string patternStr, int patternIndex)
			{
				throw new PatternParseException(pattern, index);
			}
		}
	}
}
