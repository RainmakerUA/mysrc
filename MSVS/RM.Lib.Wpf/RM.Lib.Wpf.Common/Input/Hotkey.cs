using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;

namespace RM.Lib.Wpf.Common.Input
{
	public sealed class Hotkey : IDisposable
	{
		#region Nested Type
		
		private static class NativeMethods
		{
			private const string _user32 = "user32.dll";

			[DllImport(_user32, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vkey);

			[DllImport(_user32, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		}

		#endregion

		#region Fields

		private const int _wmHotKey = 0x0312;

		private static Dictionary<int, Hotkey>? _hotkeyToCallbackProc;

		private readonly int _vkeyCode;

		private bool _disposed;

		#endregion

		#region Constructor
		
		public Hotkey(Key key, KeyModifiers keyModifiers, Action<Hotkey> action, bool register = true)
		{
			Key = key;
			KeyModifiers = keyModifiers;
			Action = action;

			_vkeyCode = KeyInterop.VirtualKeyFromKey(Key);
			ID = (int) KeyModifiers * 0x10000 + _vkeyCode;

			if (register)
			{
				Register();
			}
		}

		#endregion

		#region Properties
		
		public int ID { get; }

		public Key Key { get; }

		public KeyModifiers KeyModifiers { get; }

		public Action<Hotkey>? Action { get; }

		#endregion

		#region Public Methods
		
		public void Register()
		{
			var result = NativeMethods.RegisterHotKey(0, ID, (uint) KeyModifiers, (uint) _vkeyCode);

			if (!result)
			{
				var win32Exception = new Win32Exception(Marshal.GetLastWin32Error());
				throw new Exception($"Cannot register Hotkey: {KeyModifiers} + {Key}", win32Exception);
			}

			if (_hotkeyToCallbackProc == null)
			{
				_hotkeyToCallbackProc = [];
				ComponentDispatcher.ThreadFilterMessage += ThreadFilterMessage;
			}

			_hotkeyToCallbackProc.Add(ID, this);
		}

		public void Unregister()
		{
			if (_hotkeyToCallbackProc?.ContainsKey(ID) ?? false)
			{
				if (!NativeMethods.UnregisterHotKey(IntPtr.Zero, ID))
				{
					var win32Exception = new Win32Exception(Marshal.GetLastWin32Error());
					throw new Exception($"Cannot unregister Hotkey: {KeyModifiers} + {Key}", win32Exception);
				}

				_hotkeyToCallbackProc.Remove(ID);
			}
		}

		#endregion

		#region Dispose Pattern

		public void Dispose()
		{
			DisposeInternal();
			GC.SuppressFinalize(this);
		}

		private void DisposeInternal()
		{
			if (!_disposed)
			{
				Unregister();
				_disposed = true;
			}
		}

		~Hotkey()
		{
			DisposeInternal();
		}

		#endregion

		private static void ThreadFilterMessage(ref MSG msg, ref bool handled)
		{
			if (!handled)
			{
				if (msg.message == _wmHotKey)
				{
					if (_hotkeyToCallbackProc?.TryGetValue((int) msg.wParam, out var hotKey) ?? false)
					{
						hotKey.Action?.Invoke(hotKey);
						handled = true;
					}
				}
			}
		}
	}                   
}
