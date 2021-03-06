﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Enumerators
{
	public sealed class PatternEnumerator : IEnumerable<long>, IEnumerator<long>
	{
		private readonly Pattern _pattern;
		private readonly StreamQueue _strQueue;

		private bool _enumerationDone;
		private long _currentPosition;

		public PatternEnumerator(Stream stream, Pattern pattern)
		{
			_pattern = pattern;
			_strQueue = new StreamQueue(stream, pattern.Length);
			Reset();
		}

		#region IEnumerable

		public IEnumerator<long> GetEnumerator() => this;

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion

		#region IEnumerator

		public bool MoveNext()
		{
			return (_currentPosition = GetNextMatch()) > Helper.InvalidOffset;
		}

		public void Reset()
		{
			_enumerationDone = false;
			_currentPosition = Helper.InvalidOffset;
		}

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

		#endregion
		public void Dispose()
		{
		}

		private long GetNextMatch()
		{
			do
			{
				if (Helper.CompareBytes(_strQueue.QueuedBytes, _pattern.Bytes))
				{
					return _strQueue.SkipPatternBytes();
				}
			}
			while (_strQueue.SkipByte());

			return Helper.InvalidOffset;
		}
	}
}
