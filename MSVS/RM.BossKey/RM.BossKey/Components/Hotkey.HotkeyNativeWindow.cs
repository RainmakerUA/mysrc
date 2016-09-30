using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RM.BossKey.Win32;

namespace RM.BossKey.Components
{
	partial class Hotkey
	{
		private sealed class HotkeyNativeWindow : NativeWindow, IDisposable
		{
			private readonly Hotkey _hk;
			private GCHandle _rootHandle;

			public HotkeyNativeWindow(Hotkey hotkey)
			{
				_hk = hotkey;
			}

			#region Disposable

			~HotkeyNativeWindow()
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
					if (_rootHandle.IsAllocated)
					{
						_rootHandle.Free();
					}
				}

				// Free unmanaged resources
				DestroyHandle();
			}

			#endregion

			public HandleRef HandleRef => new HandleRef(this, Handle);

			public void Create()
			{
				_rootHandle = GCHandle.Alloc(_hk, GCHandleType.Normal);
				CreateHandle(new CreateParams());
			}

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == NativeMethods.WmHotkey)
				{
					_hk.HotkeyProc(m.WParam.ToInt32());
				}

				base.WndProc(ref m);
			}
		}
	}
}
