using System;

namespace RM.Lib.UzTicket.Model
{
	public sealed class ScanEventArgs : EventArgs
	{
		public ScanEventArgs(string callbackID, string message)
		{
			CallbackID = callbackID;
			Message = message;
		}

		// public EventType ?

		public string CallbackID { get; }

		public string Message { get; }
	}
}
