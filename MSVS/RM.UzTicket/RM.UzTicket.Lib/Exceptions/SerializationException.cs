using System;

namespace RM.UzTicket.Lib.Exceptions
{
	public class SerializationException : UzException
	{
		public SerializationException()
		{
		}

		public SerializationException(string message) : base(message)
		{
		}

		public SerializationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
