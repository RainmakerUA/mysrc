using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RM.Win.Utils.Flasher.Interop
{
	partial class Win32
	{
		[Flags]
		private enum FlashWFlags : uint
		{
			Stop = 0x0,
			Caption = 0x1,
			Tray = 0x2,
			All = Caption + Tray,
			Timer = 0x4,
			TimerNoFg = 0xC
		}

		private enum WindowLongIndex
		{
			WndProc = -4,
			HInstance = -6,
			HWndParent = -8,
			ID = -12,
			Style = -16,
			UserData = -20,
			ExStyle = -21
		}

		[Flags]
		private enum WindowStyle
		{
			// WS_VISIBLE
			Visible = 0x10000000,
		}

		[Flags]
		private enum WindowExStyle
		{
			// WS_EX_TOOLWINDOW
			ToolWindow = 0x00000080,
		}

		[StructLayout(LayoutKind.Sequential)]
		private readonly struct FlashWInfo
		{
			public readonly uint Size;

			public readonly IntPtr WindowHandle;

			public readonly FlashWFlags Flags;

			private readonly uint Count;

			private readonly uint Timeout;
			
			public FlashWInfo(IntPtr winHandle, FlashWFlags flags, uint count, uint timeout)
			{
				Size = (uint) Marshal.SizeOf<FlashWInfo>();
				WindowHandle = winHandle;
				Flags = flags;
				Count = count;
				Timeout = timeout;
			}

			public static FlashWInfo FromInfo(FlashWindowInfo info)
			{
				return new FlashWInfo(info.WindowHandle, (FlashWFlags)info.Flags, info.Count, (uint)info.Timeout.TotalMilliseconds);
			}
		}

		private static class NativeMethods
		{
			private const string _user32 = "user32.dll";

			[return: MarshalAs(UnmanagedType.Bool)]
			public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

			[DllImport(_user32)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool IsWindow(IntPtr hWnd);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool EnumWindows(EnumWindowProc lpEnumFunc, IntPtr lParam);

			[DllImport(_user32)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool FlashWindowEx([In] in FlashWInfo info);

			[DllImport(_user32, EntryPoint = "GetWindowLongW", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern int GetWindowLong(IntPtr hWnd, WindowLongIndex index);

			[DllImport(_user32, EntryPoint = "GetWindowTextW", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder text, [In] int maxLength);

			[DllImport(_user32, EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int GetClassName(IntPtr hWnd, [Out] StringBuilder text, [In] int maxLength);
		}
	}
}
