using System;
using System.Runtime.InteropServices;

namespace RM.BossKey.Win32
{
	internal static partial class NativeMethods
	{
		private const string _user32 = "user32.dll";

		[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindWindow(string lpszClass, string lpszWindow);

		[DllImport(_user32, SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PostMessage(HandleRef hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport(_user32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterHotKey(HandleRef hWnd, int id, uint fsModifiers, uint vk);

		[DllImport(_user32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterHotKey(HandleRef hWnd, int id);

		[DllImport(_user32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(HandleRef hWnd);

		[DllImport(_user32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(HandleRef hWnd, ShowWindowCommand nCmdShow);
	}
}
