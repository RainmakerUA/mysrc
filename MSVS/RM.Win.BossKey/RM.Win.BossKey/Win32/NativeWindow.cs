using System;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RM.Win.BossKey.Win32
{
	internal class NativeWindow
	{
		private const int _maxText = 1024;

		public NativeWindow(IntPtr handle)
		{
			Handle = handle;
			Title = GetWindowTitle(handle);
			Class = GetWindowClass(handle);
			Icon = GetWindowIcon(handle);
			ExeName = GetWindowProcessExecutableName(handle);
		}

		public IntPtr Handle { get; }

		public string Title { get; }

		public string Class { get; }

		public string ExeName { get; }

		public ImageSource Icon { get; }

		public static NativeWindow Find(string className, string title)
		{
			var handle = NativeMethods.FindWindow(className, title);
			return handle != IntPtr.Zero ? new NativeWindow(handle) : null;
		}

		private static string GetWindowTitle(IntPtr handle)
		{
			var sb = new StringBuilder(_maxText);
			NativeMethods.GetWindowText(handle, sb, _maxText);
			return sb.ToString();
		}

		private static string GetWindowClass(IntPtr handle)
		{
			var sb = new StringBuilder(_maxText);
			NativeMethods.GetClassName(handle, sb, _maxText);
			return sb.ToString();
		}

		private static ImageSource GetWindowIcon(IntPtr handle)
		{
			var result = NativeMethods.SendMessage(handle, NativeMethods.WM_GETICON, (IntPtr)NativeMethods.ICON_BIG, IntPtr.Zero);

			if (result == IntPtr.Zero)
			{
				result = NativeMethods.GetClassLongPtr(handle, NativeMethods.GCL_HICON);
			}

			return result != IntPtr.Zero
					? Imaging.CreateBitmapSourceFromHIcon(result, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
					: null;
		}

		private static string GetWindowProcessExecutableName(IntPtr handle)
		{
			uint processID;
			var threadID = NativeMethods.GetWindowThreadProcessId(handle, out processID);

			if (processID != 0)
			{
				var process = System.Diagnostics.Process.GetProcessById((int)processID);
				return process.MainModule.ModuleName;
			}

			return null;
		}
	}
}
