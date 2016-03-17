using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace RM.InputRobot.Native
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct MOUSEINPUT
	{
		[Flags]
		private enum MOUSEEVENTF : uint
		{
			NONE = 0x0000,
			MOVE = 0x0001,
			LEFTDOWN = 0x0002,
			LEFTUP = 0x0004,
			RIGHTDOWN = 0x0008,
			RIGHTUP = 0x0010,
			MIDDLEDOWN = 0x0020,
			MIDDLEUP = 0x0040,
			XDOWN = 0x0080,
			XUP = 0x0100,
			WHEEL = 0x0800,
			HWHEEL = 0x1000,
			MOVE_NOCOALESCE = 0x2000,
			VIRTUALDESK = 0x4000,
			ABSOLUTE = 0x8000,
		}

		private readonly int _dx;
		private readonly int _dy;
		private readonly uint _mouseData;
		private readonly MOUSEEVENTF _dwFlags;
		private readonly uint _time;
		private readonly IntPtr _dwExtraInfo;

		private MOUSEINPUT(int dx, int dy, uint mouseData, MOUSEEVENTF flags)
		{
			_dx = dx;
			_dy = dy;
			_mouseData = mouseData;
			_dwFlags = flags;
			_time = 0;
			_dwExtraInfo=IntPtr.Zero;
		}

		public static MOUSEINPUT LButtonDown() => new MOUSEINPUT(0, 0, 0, MOUSEEVENTF.LEFTDOWN);
	}
}
