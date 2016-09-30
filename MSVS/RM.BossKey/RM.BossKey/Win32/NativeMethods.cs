using System;
using System.Runtime.InteropServices;

namespace RM.BossKey.Win32
{
	internal static partial class NativeMethods
	{
		private const string _user32 = "user32.dll";

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport(_user32, SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool PostMessage(HandleRef hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport(_user32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterHotKey(HandleRef hWnd, int id, uint fsModifiers, uint vk);

		[DllImport(_user32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterHotKey(HandleRef hWnd, int id);
	}
}
