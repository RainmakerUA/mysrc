using System;
using System.Collections.Generic;
using System.Text;

namespace RM.UzTicket.Telegram
{
	public sealed class ErrorEventArgs : EventArgs
	{
		public ErrorEventArgs(Exception exception)
		{
			Exception = exception;
		}

		public Exception Exception { get; }

		public string Message => Exception?.Message;
	}
}
