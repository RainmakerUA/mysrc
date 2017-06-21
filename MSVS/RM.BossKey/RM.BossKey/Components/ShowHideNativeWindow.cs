using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RM.BossKey.Win32;

namespace RM.BossKey.Components
{
	internal sealed class ShowHideNativeWindow : NativeWindow
	{
		private bool _hidden;

		private ShowHideNativeWindow()
		{
		}

		public HandleRef HandleRef => new HandleRef(this, Handle);

		public void ToggleVisibility()
		{
			var showCommand = _hidden ? ShowWindowCommand.ShowNoActivate : ShowWindowCommand.Hide;

			if (NativeMethods.ShowWindow(HandleRef, showCommand))
			{
				_hidden = !_hidden;
			}
		}

		public static ShowHideNativeWindow Find(string className, string title)
		{
			var handle = NativeMethods.FindWindow(className, title);

			if (handle == IntPtr.Zero)
			{
				var error = Marshal.GetLastWin32Error();
				throw new Win32Exception(error);
			}

			var window = new ShowHideNativeWindow();
			window.AssignHandle(handle);

			window._hidden = !NativeMethods.IsWindowVisible(window.HandleRef);

			return window;
		}
	}
}
