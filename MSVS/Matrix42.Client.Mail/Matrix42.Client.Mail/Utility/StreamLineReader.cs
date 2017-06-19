using System;
using System.IO;
using System.Text;

namespace Matrix42.Client.Mail.Utility
{
	internal sealed class StreamLineReader : IDisposable
	{
		private const char _cr = '\r';
		private const char _lf = '\n';
		private const int _bufferSize = 0x80;
		private readonly Stream _stream;
		private readonly bool _autoClose;

		public StreamLineReader(Stream stream, bool autoClose = false)
		{
			_stream = stream;
			_autoClose = autoClose;
		}

		public string ReadLine()
		{
			var bufferPosition = 0;
			int currentByte;
			char[] buffer = null;
			StringBuilder builder = null;

			while ((currentByte = _stream.ReadByte()) > -1)
			{
				var currentChar = (char)(byte)currentByte;

				if (buffer == null)
				{
					buffer = new char[_bufferSize];
				}

				buffer[bufferPosition++] = currentChar;

				if (currentChar == _lf)
				{
					bufferPosition -= 1;

					if (bufferPosition >= 1 && buffer[bufferPosition - 1] == _cr)
					{
						bufferPosition -= 1;
					}

					break;
				}

				if (bufferPosition == _bufferSize)
				{
					if (builder == null)
					{
						builder = new StringBuilder(_bufferSize * 2);
					}

					builder.Append(buffer);
					bufferPosition = 0;
				}
			}

			if (buffer == null)
			{
				return null;
			}

			if (builder != null)
			{
				builder.Append(buffer, 0, bufferPosition);
				return builder.ToString();
			}

			return new String(buffer, 0, bufferPosition);
		}

		public void Dispose()
		{
			if (_autoClose)
			{
				_stream?.Dispose();
			}
		}
	}
}
