using System.Collections.Generic;
using System.IO;

namespace RM.BinPatcher.Enumerators
{
	internal sealed class StreamQueue
	{
		private readonly Stream _stream;
		private readonly int _queueLength;
		private readonly Queue<byte> _queue;
		private long _position;

		public StreamQueue(Stream stream, int queueLength)
		{
			_stream = stream;
			_queueLength = queueLength;
			_queue = new Queue<byte>(_queueLength);
			InitializeQueue();
		}

		public long Position => _position;

		public byte[] QueuedBytes => _queue.ToArray();

		public bool SkipByte()
		{
			var res = _stream.ReadByte();

			if (res > -1)
			{
				_queue.Dequeue();
				_queue.Enqueue((byte)res);
				_position += 1;
				return true;
			}

			return false;
		}

		public long SkipPatternBytes()
		{
			var oldPosition = _position;
			var bytes = new byte[_queueLength];

			_stream.Read(bytes, 0, _queueLength);
			_queue.Clear();

			foreach (var b in bytes)
			{
				_queue.Enqueue(b);
			}

			_position += _queueLength;

			return oldPosition;
		}

		private void InitializeQueue()
		{
			var bytes = new byte[_queueLength];

			_position = 0;
			_stream.Position = 0;
			_stream.Read(bytes, 0, _queueLength);

			foreach (var b in bytes)
			{
				_queue.Enqueue(b);
			}
		}
	}
}