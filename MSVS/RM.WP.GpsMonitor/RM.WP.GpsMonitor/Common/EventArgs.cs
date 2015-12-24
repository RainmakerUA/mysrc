using System;

namespace RM.WP.GpsMonitor.Common
{
	internal sealed class EventArgs<T> : EventArgs
	{
		public EventArgs(T data)
		{
			Data = data;
		}

		public T Data { get; }
	}
}