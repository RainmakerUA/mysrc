using System;

namespace RM.WP.GpsMonitor.Common
{
	internal sealed class EventArgs<T> : EventArgs
	{
		private readonly T _data;

		public EventArgs(T data)
		{
			_data = data;
		}

		public T Data
		{
			get { return _data; }
		}
	}
}