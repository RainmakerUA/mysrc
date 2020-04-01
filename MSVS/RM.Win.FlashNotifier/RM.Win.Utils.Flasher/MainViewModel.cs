using System;
using System.Collections.Generic;
using RM.Win.Utils.Flasher.Interop;

namespace RM.Win.Utils.Flasher
{
	public sealed class MainViewModel : BindableBase
	{
		private IReadOnlyList<WindowInfo> _windows;
		private WindowInfo? _selectedWindow;

		public MainViewModel()
		{
			RefreshWindowsCommand = new DelegateCommand<object?>(RefreshWindowsExecute);
			FlashWindowCommand = new DelegateCommand<WindowInfo>(FlashWindowExecute, FlashWindowCanExecute);
			UnflashWindowCommand = new DelegateCommand<WindowInfo>(UnflashWindowExecute, FlashWindowCanExecute);

			_windows = Win32.GetTopWindows();

			if (_windows.Count > 0)
			{
				_selectedWindow = _windows[0];
			}
		}

		public DelegateCommand<object?> RefreshWindowsCommand { get; }

		public DelegateCommand<WindowInfo> FlashWindowCommand { get; }

		public DelegateCommand<WindowInfo> UnflashWindowCommand { get; }


		public IReadOnlyList<WindowInfo> Windows
		{
			get => _windows;
			set => SetProperty(ref _windows, value);
		}

		public WindowInfo? SelectedWindow
		{
			get => _selectedWindow;
			set
			{
				if (SetProperty(ref _selectedWindow, value))
				{
					FlashWindowCommand.RaiseCanExecuteChanged();
					UnflashWindowCommand.RaiseCanExecuteChanged();
				}
			}
		}

		private void RefreshWindowsExecute(object? arg)
		{
			var windows = Win32.GetTopWindows();

			Windows = windows;
			
			if (windows.Count > 0)
			{
				SelectedWindow = windows[0];
			}
		}

		private static void FlashWindowExecute(WindowInfo info)
		{
			Win32.FlashWindow(info.Handle);
		}

		private static bool FlashWindowCanExecute(WindowInfo info)
		{
			return (info?.Handle ?? IntPtr.Zero) != IntPtr.Zero;
		}

		private static void UnflashWindowExecute(WindowInfo info)
		{
			Win32.FlashWindow(new FlashWindowInfo { WindowHandle = info.Handle, Flags = FlashWindowFlags.Stop });
		}
	}
}
