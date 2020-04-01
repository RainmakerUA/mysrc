using System;
using System.Text;

namespace RM.Win.FlashNotifier.Interop
{
	internal static partial class Win32
	{
		private const int _maxOutStringLength = 255;

		public static bool RegisterShellHookWindow(in IntPtr hWnd)
		{
			return NativeMethods.RegisterShellHookWindow(hWnd);
		}

		public static bool DeregisterShellHookWindow(in IntPtr hWnd)
		{
			return NativeMethods.DeregisterShellHookWindow(hWnd);
		}

		public static string? GetWindowText(in IntPtr hWnd)
		{
			var builder = new StringBuilder(_maxOutStringLength + 1);
			return NativeMethods.GetWindowText(hWnd, builder, _maxOutStringLength) > 0 
					? builder.ToString()
					: null;
		}

		public static string? GetWindowClassName(in IntPtr hWnd)
		{
			var builder = new StringBuilder(_maxOutStringLength + 1);
			return NativeMethods.GetClassName(hWnd, builder, _maxOutStringLength) > 0 
					? builder.ToString()
					: null;
		}
	}
}
