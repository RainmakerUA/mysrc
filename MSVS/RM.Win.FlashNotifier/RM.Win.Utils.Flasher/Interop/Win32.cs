using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RM.Win.Utils.Flasher.Interop
{
	internal static partial class Win32
	{
		private const int _maxOutStringLength = 255;

		public delegate bool EnumWindowHandler<in T>(IntPtr hWnd, T arg);

		private class EnumWindowData<T>
		{
			public readonly EnumWindowHandler<T> Handler;

			public readonly T Argument;

			public EnumWindowData(EnumWindowHandler<T> handler, T argument)
			{
				Handler = handler;
				Argument = argument;
			}
		}

		public static bool EnumerateWindows<T>(EnumWindowHandler<T> handler, T arg)
		{
			var enumData = new EnumWindowData<T>(handler, arg);
			var gcHandle = GCHandle.Alloc(enumData, GCHandleType.Normal);

			try
			{
				return NativeMethods.EnumWindows(EnumWinProc<T>, GCHandle.ToIntPtr(gcHandle));
			}
			finally
			{
				gcHandle.Free();
			}
		}

		public static IReadOnlyList<WindowInfo> GetTopWindows()
		{
			var list = new List<WindowInfo>();

			if (EnumerateWindows(StoreWindow, list))
			{
				list.Sort((win1, win2) => String.CompareOrdinal(win1.Title, win2.Title));
				return list.AsReadOnly();
			}

			return Array.Empty<WindowInfo>();

			static bool StoreWindow(IntPtr winHandle, IList<WindowInfo> list)
			{
				var winStyle = (WindowStyle) NativeMethods.GetWindowLong(winHandle, WindowLongIndex.Style);
				var winExStyle = (WindowExStyle) NativeMethods.GetWindowLong(winHandle, WindowLongIndex.ExStyle);
				var windowText = GetWindowText(winHandle);
				var windowClassName = GetWindowClassName(winHandle);

				if (winStyle.HasFlag(WindowStyle.Visible) && !winExStyle.HasFlag(WindowExStyle.ToolWindow)
					&& !String.IsNullOrWhiteSpace(windowText) && !String.IsNullOrWhiteSpace(windowClassName))
				{
					list.Add(new WindowInfo(winHandle, windowText, windowClassName));
				}

				return true;
			}
		}

		public static bool FlashWindow(IntPtr winHandle)
		{
			return FlashWindow(new FlashWindowInfo
										{
											WindowHandle = winHandle,
											Flags = FlashWindowFlags.All | FlashWindowFlags.TimerNoForeground,
											//Count = 10,
											//Timeout = //TimeSpan.FromSeconds(.1D)
										});
		}

		public static bool FlashWindow(FlashWindowInfo info)
		{
			var flashWInfo = FlashWInfo.FromInfo(info);
			return NativeMethods.FlashWindowEx(in flashWInfo);
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

		[return: MarshalAs(UnmanagedType.Bool)]
		private static bool EnumWinProc<T>(IntPtr handle, IntPtr argHandle)
		{
			var dataHandle = GCHandle.FromIntPtr(argHandle);
			var data = dataHandle.Target as EnumWindowData<T>;

			return data?.Handler?.Invoke(handle, data.Argument) ?? true;
		}
	}
}
