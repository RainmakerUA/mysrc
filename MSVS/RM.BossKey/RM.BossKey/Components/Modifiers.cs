using System;

namespace RM.BossKey.Components
{
	[Flags]
	public enum Modifiers : uint
	{
		None = 0x000,
		Alt = 0x001,
		Control = 0x0002,
		Shift = 0x0004,
		Win = 0x0008,
		[Obsolete("Not supported on Windows Vista+", true)]
		NoRepeat = 0x4000
	}
}
