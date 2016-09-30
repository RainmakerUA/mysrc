using System;

namespace RM.BossKey.Components
{
	public class EventArgs<T>: EventArgs
	{
		public EventArgs(T value)
		{
			Value = value;
		}

		public T Value { get; }
	}
}
