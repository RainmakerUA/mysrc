using System;

namespace RM.Win.Utils.Flasher.Interop
{
	[Flags]
	public enum FlashWindowFlags
	{
		Stop = 0x0,
		Caption = 0x1,
		Tray = 0x2,
		All = Caption + Tray,
		Timer = 0x4,
		TimerNoForeground = 0xC
	}

	internal sealed class FlashWindowInfo
	{
		public IntPtr WindowHandle { get; set; }

		public FlashWindowFlags Flags { get; set; }

		public uint Count { get; set; }

		public TimeSpan Timeout { get; set; }
	}
}
