using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RM.Shooter.Native;

namespace RM.Shooter
{
	internal class FrameChanger
	{
		private sealed class FrameSettings
		{
			private readonly string _wndClass;
			private readonly string _wndTitle;
			private readonly FrameMode _frameMode;
			private readonly Point? _wndPos;
			private readonly Size? _wndSize;

			public FrameSettings(string wndClass, string wndTitle,
								FrameMode frameMode, Point? wndPos, Size? wndSize)
			{
				_wndClass = wndClass;
				_wndTitle = wndTitle;
				_frameMode = frameMode;
				_wndPos = wndPos;
				_wndSize = wndSize;
			}

			public string WndClass
			{
				get { return _wndClass; }
			}

			public string WndTitle
			{
				get { return _wndTitle; }
			}

			public FrameMode FrameMode
			{
				get { return _frameMode; }
			}

			public Point WndPos
			{
				get { return _wndPos.GetValueOrDefault(); }
			}

			public Size WndSize
			{
				get { return _wndSize.GetValueOrDefault(); }
			}

			public bool AreActive
			{
				get
				{
					return !String.IsNullOrEmpty(_wndClass)
							|| !String.IsNullOrEmpty(_wndTitle);
				}
			}

			public bool ChangeFrame
			{
				get { return _frameMode != FrameMode.Unchanged; }
			}

			public bool ChangePosition
			{
				get { return _wndPos.HasValue; }
			}

			public bool ChangeSize
			{
				get { return _wndSize.HasValue; }
			}
		}

		private static readonly Dictionary<FrameMode, Tuple<WS, WS>> _frameModes = new Dictionary<FrameMode, Tuple<WS, WS>>
																				{
																					{
																						FrameMode.None,
																						Tuple.Create(
																									WS.BORDER | WS.DLGFRAME | WS.THICKFRAME,
																									WS.OVERLAPPED
																								)
																					},
																					{
																						FrameMode.Single,
																						Tuple.Create(
																									WS.THICKFRAME,
																									WS.BORDER | WS.DLGFRAME
																								)
																					},
																					{
																						FrameMode.Double,
																						Tuple.Create(
																									WS.OVERLAPPED,
																									WS.BORDER | WS.DLGFRAME | WS.THICKFRAME
																								)
																					}
																				};

		private readonly Logger _logger;
		private readonly HashSet<IntPtr> _processedWindows;
		private FrameSettings _settings;
		private uint _shellMessage;
		private int _lastError;

		public FrameChanger(string wndClass, string wndTitle,
								FrameMode wndFrame, Point? wndPos, Size? wndSize)
			: this()
		{
			ReInitialize(wndClass, wndTitle, wndFrame, wndPos, wndSize);
		}

		private FrameChanger()
		{
			_processedWindows = new HashSet<IntPtr>();
			_logger = new Logger("FrameChanger");
		}

		public void InitializeShellHook(IntPtr shellHookWnd)
		{
			_logger.Log(Logger.Level.Debug, "Initializing Shell hook");

			if ((_shellMessage = Win32.RegisterShellHookMessage(out _lastError)) != 0)
			{
				if (!Win32.RegisterShellHookWindow(shellHookWnd, out _lastError))
				{
					_shellMessage = 0;
				}
			}

			LogLastError();
		}

		public void UninitializeShellHook(IntPtr shellHookWnd)
		{
			_logger.Log(Logger.Level.Debug, "Uninitializing Shell hook");

			if (_shellMessage != 0)
			{
				Win32.DeregisterShellHookWindow(shellHookWnd, out _lastError);
			}

			LogLastError();
		}

		public void ApplyWindowFrames()
		{
			if (_settings.AreActive)
			{
				ChangeWindowFrameSettings(_settings);
			}
		}

		public void ProcessShellMessage(ref Message msg)
		{
			if (msg.Msg == _shellMessage)
			{
				if (msg.WParam.ToInt32() == (int)HSHELL.WINDOWCREATED)
				{
					UpdateWindow(msg.LParam, _settings);
				}
			}
		}

		public void ReInitialize(string wndClass, string wndTitle, FrameMode wndFrame, Point? wndPos, Size? wndSize)
		{
			_settings = new FrameSettings(wndClass, wndTitle, wndFrame, wndPos, wndSize);
		}

		private void ChangeWindowFrameSettings(FrameSettings settings)
		{
			_logger.Log(Logger.Level.Debug, "Applying frame settings for existing windows");

			Win32.EnumWindows(AddWndHandleToList, settings, out _lastError);

			LogLastError();
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		private bool AddWndHandleToList(IntPtr hWnd, object obj)
		{
			UpdateWindow(hWnd, obj as FrameSettings);
			return true;
		}

		private void UpdateWindow(IntPtr hWnd, FrameSettings settings)
		{
			if (LogCondition(settings != null, "Settings not set")
					&& LogCondition(settings.AreActive, "Settings not active")
					&& LogCondition(!_processedWindows.Contains(hWnd), "Window is already processed"))
			{
				var wndClass = settings.WndClass;
				var wndTitle = settings.WndTitle;

				if ((String.IsNullOrEmpty(wndClass) || StringsEqual(wndClass, Win32.GetWndClass(hWnd, out _lastError)))
						&& (String.IsNullOrEmpty(wndTitle) || StringsEqual(wndTitle, Win32.GetWndTitle(hWnd, out _lastError))))
				{
					var frameMode = settings.FrameMode;
					var pos = settings.WndPos;
					var size = settings.WndSize;
					var setPosFlags = SWP.NOACTIVATE | SWP.NOZORDER;

					_logger.Log(
								Logger.Level.Debug,
								"Updating window 0x{0:X}: Frame: {1}; Pos: {2}; Size: {3}",
								hWnd.ToInt64(), frameMode, pos, size
							);

					if (settings.ChangeFrame)
					{
						var winStyle = Win32.GetWindowStyle(hWnd, out _lastError);
						var styleChange = _frameModes[frameMode];
						winStyle = (winStyle & ~styleChange.Item1) | styleChange.Item2;
						Win32.SetWindowStyle(hWnd, winStyle, out _lastError);
						setPosFlags |= SWP.FRAMECHANGED;

						LogLastError();
					}

					if (!settings.ChangePosition)
					{
						setPosFlags |= SWP.NOMOVE;
					}

					if (!settings.ChangeSize)
					{
						setPosFlags |= SWP.NOSIZE;
					}

					Win32.SetWindowPos(hWnd, pos.X, pos.Y, size.Width, size.Height, setPosFlags, out _lastError);
					_processedWindows.Add(hWnd);

					LogLastError();
				}
			}
		}

		private bool LogCondition(bool condition, string falseMessage)
		{
			if (!condition)
			{
				_logger.Log(Logger.Level.Info, falseMessage);
			}

			return condition;
		}

		private void LogLastError()
		{
			if (_logger != null && _lastError > 0)
			{
				var w32exc = new Win32Exception(_lastError);
				_logger.Log(Logger.Level.Warning, "Win32 Error {0}: {1}", _lastError, w32exc.Message);
			}
		}

		private static bool StringsEqual(string str1, string str2)
		{
			return String.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
		}
	}
}
