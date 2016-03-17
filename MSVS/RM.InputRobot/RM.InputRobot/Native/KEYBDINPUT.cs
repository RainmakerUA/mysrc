using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace RM.InputRobot.Native
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct KEYBDINPUT
	{
		[Flags]
		private enum KEYEVENTF : uint
		{
			NONE = 0x0000,
			EXTENDEDKEY = 0x0001,
			KEYUP = 0x0002,
			UNICODE = 0x0004,
			SCANCODE = 0x0008,
		}

		/// <summary>
		/// Virtual Key code.  Must be from 1-254.  If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0.
		/// </summary>
		private readonly VK _wVk;

		/// <summary>
		/// A hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application.
		/// </summary>
		private readonly ushort _wScan;

		/// <summary>
		/// Specifies various aspects of a keystroke.  See the KEYEVENTF_ constants for more information.
		/// </summary>
		private readonly KEYEVENTF _dwFlags;

		/// <summary>
		/// The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.
		/// </summary>
		private readonly uint _time;

		/// <summary>
		/// An additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.
		/// </summary>
		private readonly IntPtr _dwExtraInfo;

		private KEYBDINPUT(VK vk, ushort scan, KEYEVENTF flags)
		{
			_wVk = vk;
			_wScan = scan;
			_dwFlags = flags;
			_time = 0;
			_dwExtraInfo = IntPtr.Zero;
		}

		public static KEYBDINPUT Down(VK vk) => new KEYBDINPUT(vk, 0, KEYEVENTF.NONE);

		public static KEYBDINPUT Up(VK vk) => new KEYBDINPUT(vk, 0, KEYEVENTF.KEYUP);

		public static KEYBDINPUT Char(char ch) => new KEYBDINPUT(0, ch, KEYEVENTF.UNICODE);
	}
}
