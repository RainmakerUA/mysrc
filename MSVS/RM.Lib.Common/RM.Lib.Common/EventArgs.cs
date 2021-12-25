﻿namespace RM.Lib.Common
{
	public sealed class EventArgs<T>
	{
		public EventArgs(T data)
		{
			Data = data;
		}

		public T Data { get; }
	}

	public static class EventArgs
	{
		public static EventArgs<T> Create<T>(T data) => new EventArgs<T>(data);
	}
}
