using System;

namespace RM.UzTicket.Lib.Exceptions
{
	public class ImproperlyConfigured : UzException
	{
		public ImproperlyConfigured()
		{
		}

		public ImproperlyConfigured(string message) : base(message)
		{
		}

		public ImproperlyConfigured(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}