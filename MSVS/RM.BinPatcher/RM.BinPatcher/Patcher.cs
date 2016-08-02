using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RM.BinPatcher.Enumerators;
using RM.BinPatcher.Model;

namespace RM.BinPatcher
{
    public sealed class Patcher: IDisposable
    {
	    private bool _isDisposed;

		public Patcher(Stream stream)
	    {
			Helper.ValidateStream(stream, true, false);
		    Stream = stream;
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
			    // Free managed resources (none yet)
			    _isDisposed = true;
		    }
	    }

	    public void Dispose()
	    {
		    Dispose(true);
			GC.SuppressFinalize(this);
	    }

		#endregion

	    public Stream Stream { get; }

		public IEnumerable<long> FindPattern(Pattern pattern)
		{
			CheckNotDisposed();
			return new PatternEnumerator(Stream, pattern);
	    }

	    public Task<IEnumerable<Task<long>>> FindPatternAsync(Pattern pattern)
	    {
		    return FindPatternAsync(pattern, CancellationToken.None);
	    }

		public async Task<IEnumerable<Task<long>>> FindPatternAsync(Pattern pattern, CancellationToken cancellationToken)
		{
			CheckNotDisposed();

			var result = new AsyncPatternEnumerator(Stream, pattern, cancellationToken);
			await result.FindFirstMatch();

			return result;
		}

		public bool Validate(string patch)
		{
			CheckNotDisposed();
			Helper.ValidateStream(Stream, true, true);

		    return false;
	    }
		
		public Task<bool> ValidateAsync(string patch)
	    {
			return ValidateAsync(patch, CancellationToken.None);
	    }
		
		public async Task<bool> ValidateAsync(string patch, CancellationToken cancellationToken)
		{
			CheckNotDisposed();
			Helper.ValidateStream(Stream, true, true);

		    return false;
	    }

		public bool Apply(string patch)
		{
			CheckNotDisposed();
			Helper.ValidateStream(Stream, true, true);

			return false;
		}

		public Task<bool> ApplyAsync(string patch)
	    {
			return ApplyAsync(patch, CancellationToken.None);
	    }

		public async Task<bool> ApplyAsync(string patch, CancellationToken cancellationToken)
		{
			CheckNotDisposed();
			Helper.ValidateStream(Stream, true, true);

			return false;
	    }
    }
}
