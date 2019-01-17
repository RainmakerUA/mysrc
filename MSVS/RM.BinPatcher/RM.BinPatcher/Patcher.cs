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

		public IEnumerable<long> FindPattern(Pattern pattern)
		{
			CheckNotDisposed();
			return new PatternEnumerator(_stream, pattern);
		}

		public bool Validate(Patch patch)
		{
			CheckNotDisposed();
			Helper.ValidateStream(_stream, true, true);

			return patch.Entries.Select(ValidateEntry).All(res => res.Item1);
		}

		public PatchResult Apply(Patch patch)
		{
			CheckNotDisposed();
			Helper.ValidateStream(_stream, true, true);

			foreach (var entry in patch.Entries)
			{
				var validateResult = ValidateEntry(entry);
				if (!validateResult.Item1)
				{
					return PatchResult.MakeFail(validateResult.Item2);
				}

				var applyResult = ApplyEntry(entry, validateResult.Item3);
				if (!applyResult.Item1)
				{
					return PatchResult.MakeFail(applyResult.Item2);
				}
			}

			return PatchResult.MakeSuccess();
		}

		// Tests required
		private (bool, string, long[]) ValidateEntry(PatchEntry entry)
		{
			bool isSuccess = false;
			string message = null;
			long[] addresses = null;

			switch (entry.Match)
			{
				case PatchEntry.MatchBy.Address:
					if (!entry.Address.HasValue)
					{
						// Should not happen, otherwise parser is incorrect!
						message = "Entry address not specified";
						break;
					}

					var pattern = entry.OldData;
					var patternLength = pattern.Length;
					var buffer = new byte[patternLength];

					_stream.Position = entry.Address.Value;

					var readCount = _stream.Read(buffer, 0, patternLength);

					if (readCount == patternLength && Helper.CompareBytes(buffer, pattern.Bytes))
					{
						isSuccess = true;
					}
					else
					{
						message = "Pattern do not match at its address";
					}
					break;

				case PatchEntry.MatchBy.EveryMatch:
				case PatchEntry.MatchBy.FirstMatch:
					var addrs = FindPattern(entry.OldData).ToArray();
					if (addrs.Length > 0)
					{
						isSuccess = true;
						if (entry.Match == PatchEntry.MatchBy.FirstMatch)
						{
							Array.Resize(ref addrs, 1);
						}

						addresses = addrs;
					}
					else
					{
						message = "Pattern not found";
					}
					break;

				case PatchEntry.MatchBy.SingleMatch:
					var take2 = FindPattern(entry.OldData).Take(2).ToArray();
					var take2Count = take2.Length;
					if (take2Count == 1)
					{
						isSuccess = true;
						addresses = new[] { take2[0] };
					}
					else
					{
						message = take2Count == 0 ? "Pattern not found" : "Pattern is not unique";
					}
					break;

				default:
					message = "Patch entry type not supported";
					break;
			}

			return (isSuccess, message, addresses);
		}

		private (bool, string) ApplyEntry(PatchEntry entry, long[] addresses)
		{
			try
			{
				if (entry.Match == PatchEntry.MatchBy.Address)
				{
					addresses = entry.Address.HasValue ? new[] {entry.Address.Value} : null;
				}

				if (addresses == null || addresses.Length == 0)
				{
					return (false, "No address specified!");
				}

				var newBytes = entry.NewData.Bytes;

				foreach (var address in addresses)
				{
					_stream.Position = address;

					foreach (var newByte in newBytes)
					{
						if (newByte.IsValue)
						{
							_stream.WriteByte(newByte.Value);
						}
						else
						{
							_stream.Position += 1;
						}
					}
				}

				return (true, (string)null);
			}
			catch (Exception e)
			{
				return (false, e.Message);
			}
		}
	}
}
