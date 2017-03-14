using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static RM.Win.BossKey.Win32.NativeMethods;

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
			Style = GetWindowStyle(handle);
			
			var process = GetWindowProcess(handle);
			if (process != null)
			{
				ProcessID = process.Item1;
				ExeName = process.Item2;
			}
		}

		public IntPtr Handle { get; }

		public string Title { get; }

		public string Class { get; }

		public int ProcessID { get; }

		public string ExeName { get; }

		public ImageSource Icon { get; }

		public WS Style { get; }

		public bool IsVisible => IsWindowVisible(Handle);

		public override string ToString()
		{
			return $"${Handle.ToInt64():X08} '{Title}'";
		}

		public static NativeWindow Find(string className, string title)
		{
			var handle = FindWindow(className, title);
			return handle != IntPtr.Zero ? new NativeWindow(handle) : null;
		}

		public static NativeWindow[] FindAll(Predicate<NativeWindow> predicate = null)
		{
			var list = new List<NativeWindow>();
			var pred = And(DefaultFilter, predicate);
			var tuple = Tuple.Create(list, pred);
			var gcHandle = GCHandle.Alloc(tuple, GCHandleType.Normal);
			
			try
			{
				EnumWindows(FindAllCallback, (IntPtr)gcHandle);
				return list.ToArray();
			}
			finally
			{
				gcHandle.Free();
			}
		}

		private static bool DefaultFilter(NativeWindow win)
		{
			return !String.IsNullOrWhiteSpace(win.Title) && win.Icon != null
						&& !win.Class.Equals("IME", StringComparison.InvariantCultureIgnoreCase)
						&& !win.Class.Equals("MSCTFIME UI", StringComparison.InvariantCultureIgnoreCase)
						&& win.Class.IndexOf("hidden", StringComparison.InvariantCultureIgnoreCase) < 0;
		}

		private static bool FindAllCallback(IntPtr handle, IntPtr lParam)
		{
			var gch = GCHandle.FromIntPtr(lParam);
			var tuple = gch.Target as Tuple<List<NativeWindow>, Predicate<NativeWindow>>;
			var window = new NativeWindow(handle);

			if ((tuple?.Item2?.Invoke(window)).GetValueOrDefault(true))
			{
				tuple?.Item1?.Add(window);
			}

			return true;
		}

		private static string GetWindowTitle(IntPtr handle)
		{
			var sb = new StringBuilder(_maxText);
			GetWindowText(handle, sb, _maxText);
			return sb.ToString();
		}

		private static string GetWindowClass(IntPtr handle)
		{
			var sb = new StringBuilder(_maxText);
			GetClassName(handle, sb, _maxText);
			return sb.ToString();
		}

		private static ImageSource GetWindowIcon(IntPtr handle)
		{
			const uint getIconTimeout = 200;
			uint messageResult;
			var result = SendMessageTimeout(
												handle, WM_GETICON,
												(UIntPtr)ICON_BIG, IntPtr.Zero,
												SMTO_BLOCK | SMTO_ABORTIFHUNG,
												getIconTimeout,
												out messageResult
											);

			if (result != IntPtr.Zero && messageResult != 0)
			{
				result = (IntPtr)messageResult;
			}
			else
			{
				result = GetClassLongPtr(handle, GCL_HICON);
			}

			if (result != IntPtr.Zero)
			{
				var imgSource = Imaging.CreateBitmapSourceFromHIcon(result, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				imgSource.Freeze();

				return imgSource;
			}

			return null;
		}

		private static Tuple<int, string> GetWindowProcess(IntPtr handle)
		{
			uint processID;
			var threadID = GetWindowThreadProcessId(handle, out processID);

			if (processID != 0)
			{
				var process = Process.GetProcessById((int)processID);
				return Tuple.Create((int)processID, process.MainModule.ModuleName);
			}

			return null;
		}

		private static WS GetWindowStyle(IntPtr handle)
		{
			var res = GetWindowLong(handle, GWL_STYLE);
			return (WS)(long)res;
		}

		private static Predicate<T> And<T>(Predicate<T> pr1, Predicate<T> pr2)
		{
			if (pr1 == null)
			{
				return pr2 ?? (t => true);
			}

			return pr2 == null ? pr1 : (t => pr1(t) && pr2(t));
		}
	}
}
