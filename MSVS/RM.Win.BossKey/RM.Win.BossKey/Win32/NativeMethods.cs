using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RM.Win.BossKey.Win32
{
	internal delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

	internal static class NativeMethods
	{
		private const string _user32 = "user32";

		public const int GCL_HICONSM = -34;
		public const int GCL_HICON = -14;

		public const int ICON_SMALL = 0;
		public const int ICON_BIG = 1;
		public const int ICON_SMALL2 = 2;

		public const int SW_HIDE = 0;
		public const int SW_SHOWNA = 8;

		public const int WM_GETICON = 0x7F;
		public const int WM_HOTKEY = 0x0312;

		[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumWindows(EnumWindowProc lpEnumFunc, IntPtr lParam);

		[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindWindow(string lpszClass, string lpszWindow);

		[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport(_user32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport(_user32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		[DllImport(_user32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport(_user32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

		[DllImport(_user32, CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport(_user32, SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		public static IntPtr GetWindowLong(IntPtr hwnd, int nIndex)
		{
			return IntPtr.Size == 8
					? GetWindowLongPtr64(hwnd, nIndex)
					: (IntPtr)GetWindowLongPtr32(hwnd, nIndex);
		}

		[DllImport(_user32, EntryPoint = "GetWindowLong", SetLastError = true)]
		private static extern int GetWindowLongPtr32(IntPtr hWnd, int nIndex);

		[DllImport(_user32, EntryPoint = "GetWindowLongPtr", SetLastError = true)]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		
		public static IntPtr GetClassLongPtr(IntPtr hwnd, int nIndex)
		{
			return IntPtr.Size == 8
					? (IntPtr)(long)GetClassLongPtr64(hwnd, nIndex)
					: (IntPtr)(int)GetClassLongPtr32(hwnd, nIndex);
		}

		[DllImport(_user32, EntryPoint = "GetClassLong", SetLastError = true)]
		private static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);

		[DllImport(_user32, EntryPoint = "GetClassLongPtr", SetLastError = true)]
		private static extern UIntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);
	}
}
