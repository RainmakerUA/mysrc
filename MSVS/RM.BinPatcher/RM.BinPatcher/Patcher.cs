using System;
using System.Collections.Generic;
using System.IO;
using RM.BinPatcher.Enumerators;
using RM.BinPatcher.Model;

namespace RM.BinPatcher
{
    public sealed class Patcher: IDisposable
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

		public bool Validate(string patch)
		{
			CheckNotDisposed();
			Helper.ValidateStream(_stream, true, true);

		    return false;
	    }
		
		public bool Apply(string patch)
		{
			CheckNotDisposed();
			Helper.ValidateStream(_stream, true, true);

			return false;
		}
    }
}
