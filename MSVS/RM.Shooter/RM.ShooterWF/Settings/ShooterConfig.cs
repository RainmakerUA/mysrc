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

		public ShooterConfig(ImageFormat format, int quality, string folder, string nameFormat)
		{
			Format = format;
			Quality = quality;
			Folder = folder;
			NameFormat = nameFormat;
		}

		public ImageFormat Format { get; }

		public int Quality { get; }

		public string Folder { get; }

		public string NameFormat { get; }

		public static ShooterConfig Default { get; } = new ShooterConfig(_defaultFormat, _defaultQuality, _defaultFolder, _defaultNameFormat);

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
