using System;
using System.IO;

namespace RM.BinPatcher
{
	internal static class Helper
	{
		public const long InvalidOffset = -1L;

		public static void ValidateStream(Stream stream, bool checkSeekable, bool checkWriteable)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			if (!stream.CanRead)
			{
				throw new NotSupportedException("Stream is not readable!");
			}

			if (checkSeekable && !stream.CanSeek)
			{
				throw new NotSupportedException("Stream is not seekable!");
			}

			if (checkWriteable && !stream.CanWrite)
			{
				throw new NotSupportedException("Stream is not writeable!");
			}
		}

		public static bool CompareBytes(byte[] streamBytes, byte?[] patternBytes)
		{
			var length = streamBytes.Length;

			if (patternBytes.Length != length)
			{
				return false;
			}

			for (int i = 0; i < length; i++)
			{
				if (patternBytes[i].HasValue && patternBytes[i].Value != streamBytes[i])
				{
					return false;
				}
			}

			return true;
		}
	}
}
