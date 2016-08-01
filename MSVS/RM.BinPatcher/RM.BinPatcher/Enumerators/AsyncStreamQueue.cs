using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RM.BinPatcher.Enumerators
{
	internal sealed class AsyncStreamQueue
	{
		private readonly Stream _stream;
		private readonly int _queueLength;
		private readonly CancellationToken _cancellationToken;
		private readonly Queue<byte> _queue;
		private long _position;

		public AsyncStreamQueue(Stream stream, int queueLength, CancellationToken cancellationToken)
		{
			_stream = stream;
			_queueLength = queueLength;
			_cancellationToken = cancellationToken;
			_queue = new Queue<byte>(_queueLength);
		}

		public long Position => _position;

		public byte[] QueuedBytes => _queue.ToArray();

		public Task InitializeAsync()
		{
			return InitializeQueueAsync();
		}

		public async Task<bool> SkipByteAsync()
		{
			if (_cancellationToken.IsCancellationRequested)
			{
				return false;
			}

			var bytes = new byte[1];
			var read = await _stream.ReadAsync(bytes, 0, 1, _cancellationToken);

			if (read == 1)
			{
				_queue.Dequeue();
				_queue.Enqueue(bytes[0]);
				_position += 1;
				return true;
			}

			return false;
		}

		public async Task<long> SkipPatternBytesAsync()
		{
			if (_cancellationToken.IsCancellationRequested)
			{
				return Helper.InvalidOffset;
			}

			var oldPosition = _position;
			var bytes = new byte[_queueLength];
			
			await _stream.ReadAsync(bytes, 0, _queueLength, _cancellationToken);
			_queue.Clear();

			foreach (var b in bytes)
			{
				_queue.Enqueue(b);
			}

			_position += _queueLength;

			return oldPosition;
		}

		private async Task InitializeQueueAsync()
		{
			if (!_cancellationToken.IsCancellationRequested)
			{
				var bytes = new byte[_queueLength];

				_position = 0;
				_stream.Position = 0;
				await _stream.ReadAsync(bytes, 0, _queueLength, _cancellationToken);

				foreach (var b in bytes)
				{
					_queue.Enqueue(b);
				}
			}
		}
	}
}