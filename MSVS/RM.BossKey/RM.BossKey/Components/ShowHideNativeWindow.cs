using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using RM.BossKey.Win32;

namespace RM.BossKey.Components
{
	internal sealed class ShowHideNativeWindow : NativeWindow
	{
		private bool _hidden;
		private HandleRef? _handleRef;

		private ShowHideNativeWindow()
		{
		}

		public HandleRef HandleRef => _handleRef ?? (_handleRef = new HandleRef(this, Handle)).Value;

		public bool Visible => !_hidden;

		public string Title => GetWindowText(HandleRef);

		public override string ToString()
		{
			return $"${Handle.ToInt32():X08} '{Title}'";
		}

		public bool ToggleVisibility()
		{
			var showCommand = _hidden ? ShowWindowCommand.ShowNoActivate : ShowWindowCommand.Hide;
			var oldHidden = _hidden;
			
			NativeMethods.ShowWindow(HandleRef, showCommand);
			_hidden = !NativeMethods.IsWindowVisible(HandleRef);


			return oldHidden != _hidden;
		}

		public static ShowHideNativeWindow Find(string className, string title)
		{
			var handle = NativeMethods.FindWindow(className, title);

			if (handle == IntPtr.Zero)
			{
				var error = Marshal.GetLastWin32Error();
				var message = new Win32Exception(error).Message;
				throw new Exception($"Window not found: {message} ({error})!");
			}

			var window = new ShowHideNativeWindow();
			window.AssignHandle(handle);

			window._hidden = !NativeMethods.IsWindowVisible(window.HandleRef);

			return window;
		}

		private static string GetWindowText(HandleRef handleRef)
		{
			const int maxTitle = 512;
			var sb = new StringBuilder(maxTitle);

			if (NativeMethods.GetWindowText(handleRef, sb, maxTitle) <= maxTitle)
			{
				return sb.ToString();
			}

			throw new Exception("Title is too long!");
		}
	}
}
