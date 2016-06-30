using System;

namespace RM.UzTicket.Lib.Exceptions
{
	public class UzException : Exception
	{
		public UzException()
		{
		}

		public UzException(string message) : base(message)
		{
		}

		public UzException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
