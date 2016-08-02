using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Enumerators
{
	internal sealed class AsyncPatternEnumerator: IEnumerable<Task<long>>, IEnumerator<Task<long>>
	{
		private readonly AsyncStreamQueue _aSrtQueue;
		private readonly Pattern _pattern;

		private bool _enumerationDone;
		private long _currentPosition;
		private long _nextPosition;

		public AsyncPatternEnumerator(Stream stream, Pattern pattern, CancellationToken cancellationToken)
		{
			_pattern = pattern;
			_aSrtQueue = new AsyncStreamQueue(stream, pattern.Length, cancellationToken);
			Reset();
		}

		#region IEnumerable
		
		public IEnumerator<Task<long>> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region IEnumerator
		
		public bool MoveNext()
		{
			_currentPosition = _nextPosition;
			_nextPosition = Helper.InvalidOffset;

			return _currentPosition > Helper.InvalidOffset;
		}

		public void Reset()
		{
			_enumerationDone = false;
			_currentPosition = Helper.InvalidOffset;
		}

		public Task<long> Current => GetCurrent();

		object IEnumerator.Current => Current;

		public void Dispose()
		{
			// Do nothing probably
		}

		#endregion

		public async Task FindFirstMatch()
		{
			await _aSrtQueue.InitializeAsync();
			_nextPosition = await GetNextMatchAsync();
		}

		private async Task<long> GetNextMatchAsync()
		{
			do
			{
				if (Helper.CompareBytes(_aSrtQueue.QueuedBytes, _pattern.Bytes))
				{
					return await _aSrtQueue.SkipPatternBytesAsync();
				}
			} while (await _aSrtQueue.SkipByteAsync());

			return Helper.InvalidOffset;
		}

		private async Task<long> GetCurrent()
		{
			if (_currentPosition == Helper.InvalidOffset)
			{
				throw new InvalidOperationException(_enumerationDone ? "Enumeration is finished!" : "Enumeration is not started!");
			}

			_nextPosition = await GetNextMatchAsync();
			return _currentPosition;
		}
	}
}
