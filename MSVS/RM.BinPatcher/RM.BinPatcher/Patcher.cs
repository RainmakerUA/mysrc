using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RM.BinPatcher.Enumerators;
using RM.BinPatcher.Model;

namespace RM.BinPatcher
{
	public sealed class Patcher : IDisposable
	{
		private readonly Stream _stream;
		private readonly bool _autoClose;
		private bool _isDisposed;

		public Patcher(Stream stream, bool autoClose = false)
		{
			Helper.ValidateStream(stream, true, false);
			_stream = stream;
			_autoClose = autoClose;
		}

		#region Disposable

		~Patcher()
		{
			Dispose(false);
		}

		private void CheckNotDisposed()
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		private void Dispose(bool disposing)
		{
			if (!_isDisposed && disposing)
			{
				// Free managed resources
				if (_autoClose)
				{
					_stream.Dispose();
				}

				_isDisposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		//public Stream Stream => _stream;

		public IEnumerable<long> FindPattern(Pattern pattern)
		{
			CheckNotDisposed();
			return new PatternEnumerator(_stream, pattern);
		}

		public bool Validate(Patch patch)
		{
			CheckNotDisposed();
			Helper.ValidateStream(_stream, true, true);

			return patch.Entries.Select(ValidateEntry).All(res => res.IsSuccess);
		}

		public bool Apply(Patch patch)
		{
			CheckNotDisposed();
			Helper.ValidateStream(_stream, true, true);

			return false;
		}
		
		// Tests required
		private PatchResult ValidateEntry(PatchEntry entry)
		{
			switch (entry.Match)
			{
				case PatchEntry.MatchBy.Address:
					if (!entry.Address.HasValue)
					{
						// Should not happen, otherwise parser is incorrect!
						return PatchResult.MakeFail("Entry address not specified");
					}

					var pattern = entry.OldData;
					var patternLength = pattern.Length;
					var buffer = new byte[patternLength];

					_stream.Position = entry.Address.Value;

					var readCount = _stream.Read(buffer, 0, patternLength);

					return readCount == patternLength && Helper.CompareBytes(buffer, pattern.Bytes)
							? PatchResult.MakeSuccess()
							: PatchResult.MakeFail("Pattern do not match at its address");

				case PatchEntry.MatchBy.EveryMatch:
				case PatchEntry.MatchBy.FirstMatch:
					return FindPattern(entry.OldData).Any()
							? PatchResult.MakeSuccess()
							: PatchResult.MakeFail("Pattern not found");

				case PatchEntry.MatchBy.SingleMatch:
					var take2Count = FindPattern(entry.OldData).Take(2).Count();
					return take2Count == 1
							? PatchResult.MakeSuccess()
							: PatchResult.MakeFail(take2Count == 0 ? "Pattern not found" : "Pattern is not unique");
				default:
					return PatchResult.MakeFail("Patch entry type not supported");
			}
		}
	}
}
