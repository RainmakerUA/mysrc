using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RM.Shooter.Native
{
	internal static class Win32
	{
		internal delegate bool EnumWindowProc(IntPtr hWnd, object lParam);

		private static class SafeNativeMethods
		{
			private const string _user32 = "user32";

			public const int GWL_STYLE = -16;

			[DllImport(_user32)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool IsWindow(IntPtr hWnd);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool EnumWindows(EnumWindowProc lpEnumFunc, object lParam);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

			[DllImport(_user32, SetLastError = true)]
			public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

			[DllImport(_user32, SetLastError = true)]
			public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

			[DllImport(_user32, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			public static extern uint RegisterWindowMessage(string lpString);

			[DllImport(_user32, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool RegisterShellHookWindow(IntPtr hWnd);

			[DllImport(_user32, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DeregisterShellHookWindow(IntPtr hWnd);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			public static extern IntPtr FindWindow(string lpszClass, string lpszWindow);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

			[DllImport(_user32, CharSet = CharSet.Auto, SetLastError = true)]
			public static extern IntPtr SetActiveWindow(IntPtr hWnd);

			[DllImport(_user32, CharSet = CharSet.Auto)]
			public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

			[DllImport(_user32, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
		}
		
		private const int _maxText = 1024;

		private static readonly IntPtr _zeroPtr = IntPtr.Zero;

		#region Window Style & Position

		public static IntPtr GetWindow(string wndClass, string wndText, out int lastError)
		{
			var result = SafeNativeMethods.FindWindow(wndClass, wndText);
			lastError = result.ToInt32() > 0 ? 0 : Marshal.GetLastWin32Error();

			return result;
		}

		public static IntPtr GetChildWindow(IntPtr hWndParent, string wndClass, string wndText, out int lastError)
		{
			var result = SafeNativeMethods.FindWindowEx(hWndParent, _zeroPtr, wndClass, wndText);
			lastError = result.ToInt32() > 0 ? 0 : Marshal.GetLastWin32Error();

			return result;
		}

		public static bool IsWindow(IntPtr hWnd)
		{
			return SafeNativeMethods.IsWindow(hWnd);
		}

		public static string GetWndClass(IntPtr hWnd, out int lastError)
		{
			var sb = new StringBuilder(_maxText);
			var result = SafeNativeMethods.GetClassName(hWnd, sb, _maxText);
			lastError = result > 0 ? 0 : Marshal.GetLastWin32Error();

			return result > 0 ? sb.ToString() : null;
		}

		public static string GetWndTitle(IntPtr hWnd, out int lastError)
		{
			var sb = new StringBuilder(_maxText);
			var result = SafeNativeMethods.GetWindowText(hWnd, sb, _maxText);
			lastError = result > 0 ? 0 : Marshal.GetLastWin32Error();

			return result > 0 ? sb.ToString() : null;
		}

		public static bool EnumWindows(EnumWindowProc enumProc, object obj, out int lastError)
		{
			var result = SafeNativeMethods.EnumWindows(enumProc, obj);
			lastError = result ? 0 : Marshal.GetLastWin32Error();

			return result;
		}

		public static WS GetWindowStyle(IntPtr hWnd, out int lastResult)
		{
			var result = SafeNativeMethods.GetWindowLong(hWnd, SafeNativeMethods.GWL_STYLE);
			lastResult = result != 0 ? 0 : Marshal.GetLastWin32Error();

			return (WS)result;
		}

		public static WS SetWindowStyle(IntPtr hWnd, WS newStyle, out int lastResult)
		{
			var result = SafeNativeMethods.SetWindowLong(hWnd, SafeNativeMethods.GWL_STYLE, (int)newStyle);
			lastResult = result != 0 ? 0 : Marshal.GetLastWin32Error();

			return (WS)result;
		}

		public static bool SetWindowPos(IntPtr hWnd, int x, int y, int cx, int cy, SWP flags, out int lastError)
		{
			var result = SafeNativeMethods.SetWindowPos(hWnd, _zeroPtr, x, y, cx, cy, (int)flags);
			lastError = result ? 0 : Marshal.GetLastWin32Error();

			return result;
		}

		public static bool ActivateWindow(IntPtr hWnd, out int lastError)
		{
			var result = SafeNativeMethods.SetActiveWindow(hWnd);
			var success = result.ToInt32() > 0;
			lastError = success ? 0 : Marshal.GetLastWin32Error();

			return success;
		}

		#endregion

		#region Shell Hook

		public static uint RegisterShellHookMessage(out int lastError)
		{
			const string shellMessageName = "SHELLHOOK";
			var result = SafeNativeMethods.RegisterWindowMessage(shellMessageName);
			lastError = result != 0 ? 0 : Marshal.GetLastWin32Error();

			return result;
		}

		public static bool RegisterShellHookWindow(IntPtr hWnd, out int lastError)
		{
			var result = SafeNativeMethods.RegisterShellHookWindow(hWnd);
			lastError = result ? 0 : Marshal.GetLastWin32Error();

			return result;
		}

		public static bool DeregisterShellHookWindow(IntPtr hWnd, out int lastError)
		{
			var result = SafeNativeMethods.DeregisterShellHookWindow(hWnd);
			lastError = result ? 0 : Marshal.GetLastWin32Error();

			return result;
		}

		#endregion

		#region Controls

		public static bool ClickButton(IntPtr hBtn, out int lastError)
		{
			const uint BM_CLICK = 0x00F0;

			var result = SafeNativeMethods.PostMessage(hBtn, BM_CLICK, _zeroPtr, _zeroPtr);
			lastError = result ? 0 : Marshal.GetLastWin32Error();

			return result;
		}

		#endregion
	}
}