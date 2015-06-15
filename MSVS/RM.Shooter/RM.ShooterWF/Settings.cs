using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using RM.Shooter.Configuration;

namespace RM.Shooter
{
	public enum ImageFormat
	{
		None = 0,
		Bmp,
		Jpeg,
		Png
	}

	public enum FrameMode
	{
		Unchanged = 0,
		None,
		Single,
		Double
	}

	public interface ISettings
	{
		ImageFormat Format { get; }

		int Quality { get; }

		string Folder { get; }

		string NameFormat { get; }

		// Frameless functionality

		string WindowClass { get; }

		string WindowTitle { get; }

		FrameMode FrameMode { get; }

		Point? WindowPosition { get; }

		Size? WindowSize { get; }

		void Reload();
	}

	internal static class Settings
	{
		private class HardcodedSettings : ISettings
		{
			public ImageFormat Format { get; set; }

			public int Quality { get; set; }

			public string Folder { get; set; }

			public string NameFormat { get; set; }

			public string WindowClass { get; set; }

			public string WindowTitle { get; set; }

			public FrameMode FrameMode { get; set; }

			public Point? WindowPosition { get; set; }

			public Size? WindowSize { get; set; }

			public void Reload()
			{
			}
		}

		private class IniSettings : ISettings
		{
			private readonly string _fileName;

			private HardcodedSettings _cachedSettings;

			public IniSettings(string fileName)
			{
				_fileName = fileName;
			}

			#region Properties

			public ImageFormat Format
			{
				get { return CachedSettings.Format; }
			}

			public int Quality
			{
				get { return CachedSettings.Quality; }
			}

			public string Folder
			{
				get { return CachedSettings.Folder; }
			}

			public string NameFormat
			{
				get { return CachedSettings.NameFormat; }
			}

			public string WindowClass
			{
				get { return CachedSettings.WindowClass; }
			}

			public string WindowTitle
			{
				get { return CachedSettings.WindowTitle; }
			}

			public FrameMode FrameMode
			{
				get { return CachedSettings.FrameMode; }
			}

			public Point? WindowPosition
			{
				get { return CachedSettings.WindowPosition; }
			}

			public Size? WindowSize
			{
				get { return CachedSettings.WindowSize; }
			}

			private HardcodedSettings CachedSettings
			{
				get
				{
					if (_cachedSettings == null)
					{
						Reload();
					}

					return _cachedSettings;
				}
			}

			#endregion

			public void Reload()
			{
				if (!File.Exists(_fileName))
				{
					throw new FileNotFoundException("INI-file is not found!", _fileName);
				}

				var ini = new IniFile(_fileName);
				var mainSection = ini["General"];
				var frameSection = ini["Frameless"];

				ImageFormat format;
				int quality;
				string folder;
				string nameFormat;

				FrameMode winFrame;

				_cachedSettings = new HardcodedSettings
									{
										Format = Enum.TryParse(mainSection["Format"], true, out format) && format != ImageFormat.None
													? format
													: _defaultFormat,
										Quality = Int32.TryParse(mainSection["Quality"], NumberStyles.Integer, CultureInfo.InvariantCulture, out quality)
														&& quality > 0
													? quality : _defaultQuality,
										Folder = !String.IsNullOrEmpty(folder = mainSection["Folder"])
													? folder : _defaultFolder,
										NameFormat = !String.IsNullOrEmpty(nameFormat = mainSection["NameFormat"])
													? nameFormat : _defaultNameFormat,

										WindowClass = frameSection["WndClass"],
										WindowTitle = frameSection["WndTitle"],
										FrameMode = Enum.TryParse(frameSection["WndFrame"], true, out winFrame)
													? winFrame : FrameMode.Unchanged,
										WindowPosition = ParsePoint(frameSection["WndPos"]),
										WindowSize = ParseSize(frameSection["WndSize"])
									};
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

		private const string _iniFile = "config.ini";

		private const ImageFormat _defaultFormat = ImageFormat.Bmp;
		private const int _defaultQuality = 80;
		private const string _defaultFolder = ".";
		private const string _defaultNameFormat = "{0}_{1}";

		private static readonly ISettings _hardcodedSettings = new HardcodedSettings
																{
																	Format = _defaultFormat,
																	Quality = _defaultQuality,
																	Folder = _defaultFolder,
																	NameFormat = _defaultNameFormat
																};

		public static ISettings GetSettings(string iniFile = null)
		{
			if (String.IsNullOrEmpty(iniFile))
			{
				iniFile = GetIniFileName();
			}

			if (File.Exists(iniFile))
			{
				return new IniSettings(iniFile);
			}

			return _hardcodedSettings;
		}


		private static string GetIniFileName()
		{
			var exePath = Path.GetFullPath(Assembly.GetEntryAssembly().Location);
			var exeDir = Path.GetDirectoryName(exePath) ?? String.Empty;

			return Path.Combine(exeDir, _iniFile);
		}
	}
}
