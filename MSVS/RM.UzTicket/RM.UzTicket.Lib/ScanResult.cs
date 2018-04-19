using System;

namespace RM.UzTicket.Lib
{
	public class ScanResult<T> : EventArgs
	{
		public ScanResult(string scanID, T data)
		{
			ScanID = scanID;
			Data = data;
		}

		public string ScanID { get; }

		public T Data { get; }
	}
}
