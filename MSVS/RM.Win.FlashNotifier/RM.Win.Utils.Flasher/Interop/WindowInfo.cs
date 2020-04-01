using System;

namespace RM.Win.Utils.Flasher.Interop
{
	public sealed class WindowInfo
	{
		public WindowInfo(IntPtr handle, string? title, string? @class)
		{
			Handle = handle;
			Title = title;
			Class = @class;
		}

		public IntPtr Handle { get; }

		public string? Title { get; }

		public string? Class { get; }

		public string ToString(string format)
		{
			return String.Format(format, Title, Class, Handle);
		}

		public override string ToString()
		{
			return ToString("{0} ({1}) | {2}");
		}
	}
}
