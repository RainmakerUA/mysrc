using System;
using System.Collections;
using System.Collections.Generic;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Enumerators
{
	public sealed class PatternEnumeratorSpan : IEnumerable<long>, IEnumerator<long>
	{
		private readonly ReadOnlyMemory<byte> _memory;
		private readonly Pattern _pattern;
		private readonly int _patternLength;

		private bool _enumerationDone;
		private long _currentPosition;

		public PatternEnumeratorSpan(byte[] arr, Pattern pattern) : this(arr.AsMemory(), pattern)
		{
		}

		public PatternEnumeratorSpan(ReadOnlyMemory<byte> mem, Pattern pattern)
		{
			_memory = mem;
			_pattern = pattern;
			_patternLength = _pattern.Length;
			Reset();
		}

		#region Interface implementations

		public long Current
		{
			get
			{
				if (_currentPosition == Helper.InvalidOffset)
				{
					throw new InvalidOperationException(_enumerationDone ? "Enumeration is finished!" : "Enumeration is not started!");
				}

				return _currentPosition;
			}
		}

		object IEnumerator.Current => Current;


		public IEnumerator<long> GetEnumerator() => this;

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool MoveNext() => (_currentPosition = GetNextMatch()) > Helper.InvalidOffset;

		public void Reset()
		{
			_enumerationDone = false;
			_currentPosition = Helper.InvalidOffset;
		}

		public void Dispose()
		{
		}

		#endregion Impls
		
		private long GetNextMatch()
		{
			var positionFound = false;
			var pos = _currentPosition;

			while (++pos + _patternLength <= _memory.Length
					&& !(positionFound = Helper.CompareBytes(_memory.Span.Slice((int)pos, _patternLength), _pattern.Bytes)))
			{	
			}

			return positionFound ? pos : Helper.InvalidOffset;
		}
	}
}
