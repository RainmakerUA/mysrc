using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RM.Win.FlashNotifier.Interop
{
	internal static partial class Win32
	{
		private static class NativeMethods
		{
			private const string _user32 = "user32.dll";

			[DllImport(_user32, EntryPoint = "RegisterWindowMessageW", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern uint RegisterWindowMessage([In] string str);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool RegisterShellHookWindow([In] IntPtr hWnd);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DeregisterShellHookWindow([In] IntPtr hWnd);

			[DllImport(_user32, EntryPoint = "GetWindowTextW", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder text, [In] int maxLength);

			[DllImport(_user32, EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int GetClassName(IntPtr hWnd, [Out] StringBuilder text, [In] int maxLength);
		}
	}
}
