using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Interop;
using RM.Win.FlashNotifier.Common;
using RM.Win.FlashNotifier.Interop;
using RM.Win.FlashNotifier.Utility;

namespace RM.Win.FlashNotifier
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Default skip timeout 10 seconds
		private const int _skipTimeoutSeconds = 10;

		private static readonly DependencyPropertyKey _isDisabledPropertyKey;

		public static readonly DependencyProperty IsDisabledPropertyKeyProperty;

		private readonly Cache<IntPtr, object?> _skipped;
		private NotifyWindow? _notifyWindow;
		private IntPtr? _currentWindowHandle;

		private HwndSource? _hwndSource;

		static MainWindow()
		{
			_isDisabledPropertyKey = DependencyProperty.RegisterReadOnly(
																			"IsDisabled",
																			typeof(bool),
																			typeof(MainWindow),
																			new PropertyMetadata(true)
																		);
			IsDisabledPropertyKeyProperty = _isDisabledPropertyKey.DependencyProperty;
		}

		public MainWindow()
		{
			InitializeComponent();

			_skipped = new Cache<IntPtr, object?>();
		}

		private NotifyWindow NotifyWindow => _notifyWindow ??= CreateNotifyWindow();
		
		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			var winData = FileStorage.Load<WindowData>();

			if (winData != null)
			{
				TextBoxTitles.SetLines(winData.Titles);
				TextBoxClasses.SetLines(winData.Classes);
			}

			_hwndSource = PresentationSource.FromVisual(this) as HwndSource;

			if (_hwndSource != null)
			{
				_hwndSource.AddHook(MessageHookProc);
				Win32.RegisterShellHookWindow(_hwndSource.Handle);
				SetValue(_isDisabledPropertyKey, false);
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			if (_hwndSource != null)
			{
				Win32.DeregisterShellHookWindow(_hwndSource.Handle);
				_hwndSource.RemoveHook(MessageHookProc);
			}

			_notifyWindow?.Close();

			FileStorage.Store(new WindowData { Titles = TextBoxTitles.GetLines(), Classes = TextBoxClasses.GetLines() });
		}

		private IntPtr MessageHookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == Win32.WM_SHELLHOOKMESSAGE && wParam.ToInt32() == Win32.HSHELL_FLASH)
			{
				var hWnd = lParam;
				var winTitle = Win32.GetWindowText(hWnd);
				var winClass = Win32.GetWindowClassName(hWnd);

				if (Dispatcher.CheckAccess())
				{
					HandleFlashingWindow(hWnd, winTitle, winClass);
				}
				else
				{
					Dispatcher.Invoke(new Action<IntPtr, string?, string?>(HandleFlashingWindow), hWnd, winTitle, winClass);
				}

				handled = true;
				return IntPtr.Zero;
			}

			handled = false;
			return IntPtr.Zero;
		}
		private void OnNotifyWindowClicked(object? sender, EventArgs e)
		{
			if (sender is NotifyWindow notifyWindow && _currentWindowHandle.HasValue)
			{
				notifyWindow.Hide();
				//notifyWindow.WindowTitle = String.Empty;
				//notifyWindow.WindowClass = String.Empty;
				_skipped.AddOrUpdate(_currentWindowHandle.Value, null, _skipTimeoutSeconds);
				_currentWindowHandle = null;
			}
		}

		private void HandleFlashingWindow(IntPtr windowHandle, string? windowTitle, string? windowClass)
		{
			if (IsNotificationEnabled(windowHandle, windowTitle, windowClass))
			{
				StartNotification(windowHandle, windowTitle, windowClass);
			}
		}

		private bool IsNotificationEnabled(IntPtr windowHandle, string? windowTitle, string? windowClass)
		{
			var titles = TextBoxTitles.GetLines();
			var classes = TextBoxClasses.GetLines();
			return windowHandle != IntPtr.Zero && !_skipped.Exists(windowHandle)
										&& (titles.Length == 0 && classes.Length == 0
																					|| HasLineMatching(windowTitle, titles)
																					|| HasLineMatching(windowClass, classes));

			static bool HasLineMatching(string? text, string[] patterns)
			{
				return !String.IsNullOrEmpty(text) && Array.FindIndex(patterns, MatchPattern) >= 0;

				bool MatchPattern(string pat)
				{
					if (!String.IsNullOrWhiteSpace(pat))
					{
						try
						{
							return Regex.IsMatch(
												text, pat,
												RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace
											);
						}
						catch (ArgumentException)
						{
							// Ignore pattern parse errors
						}
					}

					return false;
				}
			}
		}

		private void StartNotification(IntPtr windowHandle, string? windowTitle, string? windowClass)
		{
			if (_currentWindowHandle.Equals(windowHandle))
			{
				NotifyWindow.Visible = !NotifyWindow.Visible;
			}
			else
			{
				_currentWindowHandle = windowHandle;
				NotifyWindow.WindowTitle = windowTitle;
				NotifyWindow.WindowClass = windowClass;
				NotifyWindow.InvalidateVisual();
				NotifyWindow.Show();
			}
		}

		private NotifyWindow CreateNotifyWindow()
		{
			var notifyWindow = new NotifyWindow();
			notifyWindow.WindowClicked += OnNotifyWindowClicked;

			return notifyWindow;
		}
	}
}
