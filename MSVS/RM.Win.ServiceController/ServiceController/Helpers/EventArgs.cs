using System;

namespace ServiceController.Helpers
{
	internal class EventArgs<T> : EventArgs
	{
		private readonly T _arg;

		public EventArgs(T arg)
		{
			_arg = arg;
		}

		public T Arg
		{
			get
			{
				return _arg;
			}
		}
	}
}
