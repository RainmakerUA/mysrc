using System;

namespace RM.UzTicket.Lib
{
	public class UzTicketScanResult<T> : EventArgs
	{
		public UzTicketScanResult(string scanID, T data)
		{
			ScanID = scanID;
			Data = data;
		}

		public string ScanID { get; }

		public T Data { get; }
	}
}
