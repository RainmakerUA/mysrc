using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RM.BossKey.Win32;

namespace RM.BossKey.Components
{
	internal partial class Hotkey : IDisposable
	{
		private readonly HotkeyNativeWindow _win;
		private bool _disposed;
		private int _nextID;

		public Hotkey()
		{
			_win = new HotkeyNativeWindow(this);
			_win.Create();
		}

		#region Disposable

		~Hotkey()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				// Free managed resources
				_win.Dispose();
			}

			// Free unmanaged resources
			Unregister();

			_disposed = true;
		}

		#endregion

		public event EventHandler<EventArgs<int>> Press;

		public int Register(Modifiers mods, Keys key)
		{
			CheckDisposed();

			var id = _nextID++;

			if (!NativeMethods.RegisterHotKey(_win.HandleRef, id, (uint)mods, (uint)key))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}

			return id;
		}

		public bool Unregister(int id)
		{
			CheckDisposed();

			return NativeMethods.UnregisterHotKey(_win.HandleRef, id);
		}

		public void Unregister()
		{
			CheckDisposed();
		}

		private void CheckDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(nameof(Hotkey));
			}
		}

		private void HotkeyProc(int hkID)
		{
			Press?.Invoke(this, new EventArgs<int>(hkID));
		}
	}
}
