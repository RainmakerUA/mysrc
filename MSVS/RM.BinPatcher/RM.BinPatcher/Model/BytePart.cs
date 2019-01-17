using System;

namespace RM.BinPatcher.Model
{
	public readonly struct BytePart
	{
		[Flags]
		private enum Kind : byte
		{
			Any = 0,
			Low = 1,
			High = 2,
		}

		public static BytePart Any = new BytePart();

		private const char _question = '?';

		private static readonly char[] _hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

		private readonly Kind _kind;

		private readonly byte _value;

		private BytePart(Kind kind, byte value)
		{
			_kind = kind;
			_value = value;
		}

		public bool IsValue => _kind == (Kind.Low | Kind.High);

		public byte Value => IsValue ? _value : throw new InvalidOperationException("BytePart is a wildcard!");

		public bool IsMatch(byte other)
		{
			switch (_kind)
			{
				case Kind.Any:
					return true;

				case Kind.Low:
					return (other & 0x0F) == _value;

				case Kind.High:
					return (other & 0xF0) == _value << 4;

				case Kind.Low | Kind.High:
					return other == _value;

				default:
					throw new InvalidOperationException("Unsupported kind!");
			}
		}

		public override string ToString()
		{
			switch (_kind)
			{
				case Kind.Any:
					return new string(_question, 2);

				case Kind.Low:
					return new string(new []{ _question, _hex[_value] });

				case Kind.High:
					return new string(new[] { _hex[_value], _question });

				case Kind.Low | Kind.High:
					return _value.ToString("X2");

				default:
					throw new InvalidOperationException("Unsupported kind!");
			}
		}

		public static BytePart Low(byte lowPart)
		{
			CheckIsNibble(lowPart);
			return new BytePart(Kind.Low, lowPart);
		}

		public static BytePart High(byte highPart)
		{
			CheckIsNibble(highPart);
			return new BytePart(Kind.High, highPart);
		}

		public static BytePart Full(byte value) => new BytePart(Kind.Low | Kind.High, value);

		private static void CheckIsNibble(byte value)
		{
			if ((value & 0xF0) != 0)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Value must be in a half-byte (nibble) range (0..15)!");
			}
		}
	}
}
