using System;

namespace RM.UzTicket.Lib.Exceptions
{
	public class FailedObtainToken : UzException
	{
		public FailedObtainToken()
		{
		}

		public FailedObtainToken(string message) : base(message)
		{
		}

		public FailedObtainToken(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
