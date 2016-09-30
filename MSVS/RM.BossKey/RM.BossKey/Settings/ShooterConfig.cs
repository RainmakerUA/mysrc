using System;
using System.Globalization;
using RM.Shooter.Configuration;

namespace RM.Shooter.Settings
{
	internal class ShooterConfig
	{
		private const ImageFormat _defaultFormat = ImageFormat.Bmp;
		private const int _defaultQuality = 80;
		private const string _defaultFolder = ".";
		private const string _defaultNameFormat = "{0}_{1}";

		private static readonly ShooterConfig _default = new ShooterConfig(_defaultFormat, _defaultQuality, _defaultFolder, _defaultNameFormat);

		private readonly ImageFormat _format;
		private readonly int _quality;
		private readonly string _folder;
		private readonly string _nameFormat;

		public ShooterConfig(ImageFormat format, int quality, string folder, string nameFormat)
		{
			_format = format;
			_quality = quality;
			_folder = folder;
			_nameFormat = nameFormat;
		}

		public ImageFormat Format
		{
			get { return _format; }
		}

		public int Quality
		{
			get { return _quality; }
		}

		public string Folder
		{
			get { return _folder; }
		}

		public string NameFormat
		{
			get { return _nameFormat; }
		}

		public static ShooterConfig Default
		{
			get { return _default; }
		}

		public static ShooterConfig FromIni(IKeyedStorage<string, string> section)
		{
			ImageFormat format;
			int quality;
			string folder;
			string nameFormat;

			return new ShooterConfig(
								Enum.TryParse(section["Format"], true, out format) && format != ImageFormat.None ? format : _defaultFormat,
								Int32.TryParse(section["Quality"], NumberStyles.Integer, CultureInfo.InvariantCulture, out quality) && quality > 0 ? quality : _defaultQuality,
								!String.IsNullOrEmpty(folder = section["Folder"]) ? folder : _defaultFolder,
								!String.IsNullOrEmpty(nameFormat = section["NameFormat"]) ? nameFormat : _defaultNameFormat
							);
		}
	}
}
