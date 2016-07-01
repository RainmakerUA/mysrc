using System;
using System.Threading;

namespace RM.UzTicket.Lib.Utils
{
	internal sealed class AsyncLock : IDisposable
	{
		private readonly AutoResetEvent _value;

		public AsyncLock(AutoResetEvent value, int milliseconds = Timeout.Infinite)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			value.WaitOne(milliseconds);
			_value = value;
		}

		void IDisposable.Dispose()
		{
			_value.Set();
		}
	}
}
