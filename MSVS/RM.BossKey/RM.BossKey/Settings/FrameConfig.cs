using System;
using System.Drawing;
using RM.Shooter.Configuration;

namespace RM.Shooter.Settings
{
	internal class FrameConfig
	{
		private readonly string _windowClass;
		private readonly string _windowTitle;
		private readonly FrameMode _frameMode;
		private readonly Point? _windowPosition;
		private readonly Size? _windowSize;

		public FrameConfig(string windowClass, string windowTitle, FrameMode frameMode, Point? windowPosition, Size? windowSize)
		{
			_windowClass = windowClass;
			_windowTitle = windowTitle;
			_frameMode = frameMode;
			_windowPosition = windowPosition;
			_windowSize = windowSize;
		}

		public string WindowClass
		{
			get { return _windowClass; }
		}

		public string WindowTitle
		{
			get { return _windowTitle; }
		}

		public FrameMode FrameMode
		{
			get { return _frameMode; }
		}

		public Point? WindowPosition
		{
			get { return _windowPosition; }
		}

		public Size? WindowSize
		{
			get { return _windowSize; }
		}

		public static FrameConfig FromIni(IKeyedStorage<string, string> section)
		{
			FrameMode winFrame;

			return new FrameConfig(
								section["WndClass"],
								section["WndTitle"],
								Enum.TryParse(section["WndFrame"], true, out winFrame) ? winFrame : FrameMode.Unchanged,
								ParsePoint(section["WndPos"]),
								ParseSize(section["WndSize"])
							);
		}

		private static Point? ParsePoint(string str)
		{
			if (String.IsNullOrEmpty(str))
			{
				return null;
			}

			int x;
			int y;

			var parts = str.Split(',');

			switch (parts.Length)
			{
				case 0:
					return null;

				case 1:
					x = SafeParseInt32(parts[0]);
					return new Point(x, x);

				default:
					x = SafeParseInt32(parts[0]);
					y = SafeParseInt32(parts[1]);
					return new Point(x, y);
			}
		}

		private static Size? ParseSize(string str)
		{
			var point = ParsePoint(str);
			return point.HasValue ? new Size(point.Value) : new Size?();
		}

		private static int SafeParseInt32(string str, int defaultValue = 0)
		{
			int res;
			return Int32.TryParse(str, out res) ? res : defaultValue;
		}
	}
}
