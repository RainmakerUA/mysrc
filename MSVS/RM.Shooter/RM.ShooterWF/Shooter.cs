using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ImageFormatClass = System.Drawing.Imaging.ImageFormat;

namespace RM.Shooter
{
	internal static class Shooter
	{
		private static readonly Logger _logger = new Logger("Shooter");

		public static void CreateShot(ref string filename, ImageFormat format, int quality = 0)
		{
			ImageFormatClass imageFormat;
			EncoderParameters encoderParameters = null;

			_logger.Log(
						Logger.Level.Info,
						"Shooting screen to '{0}' (format: {1}, quality: {2})",
						filename, format, quality
					);

			switch (format)
			{
				case ImageFormat.Bmp:
					imageFormat = ImageFormatClass.Bmp;
					break;

				case ImageFormat.Jpeg:
					imageFormat = ImageFormatClass.Jpeg;
					encoderParameters = GetJpegQualityParameters(quality);
					break;

				case ImageFormat.Png:
					imageFormat = ImageFormatClass.Png;
					break;

				default:
					throw new NotSupportedException("Image format is not supported!");
			}

			try
			{
				using (var bm = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height))
				{
					using (var gr = Graphics.FromImage(bm))
					{
						var codecInfo = GetCodec(imageFormat);
						var ext = GetFileExtension(codecInfo);

						filename = Path.ChangeExtension(filename, ext);
						gr.CopyFromScreen(0, 0, 0, 0, bm.Size);
						bm.Save(filename, codecInfo, encoderParameters);

						_logger.Log(Logger.Level.Info, "Screenshot saved to {0}", filename);
					}
				}
			}
			catch (Exception e)
			{
				_logger.Log(Logger.Level.Error, e);
				throw;
			}
		}

		private static ImageCodecInfo GetCodec(ImageFormatClass format)
		{
			var codecs = ImageCodecInfo.GetImageDecoders();
			return Array.Find(codecs, c => c.FormatID == format.Guid);
		}

		private static EncoderParameters GetJpegQualityParameters(int quality)
		{
			var encoder = Encoder.Quality;
			var encoderParams = new EncoderParameters(1);
			var qualityParam = new EncoderParameter(encoder, quality);

			encoderParams.Param[0] = qualityParam;
			return encoderParams;
		}

		private static string GetFileExtension(ImageCodecInfo codecInfo)
		{
			var extensions = codecInfo.FilenameExtension.Split(';');
			if (extensions.Length > 0)
			{
				var ext = extensions[0];
				if (ext.StartsWith("*."))
				{
					ext = ext.Substring(2);
				}
				else if(ext.StartsWith("."))
				{
					ext = ext.Substring(1);
				}

				return ext.ToLowerInvariant();
			}

			return null;
		}

		/*
		private static void GetSupportedParameters(System.Drawing.Imaging.ImageFormat format)
		{
			Bitmap bitmap1 = new Bitmap(1, 1);
			ImageCodecInfo jpgEncoder = GetEncoder(format);
			EncoderParameters paramList = bitmap1.GetEncoderParameterList(jpgEncoder.Clsid);
			EncoderParameter[] encParams = paramList.Param;
			StringBuilder paramInfo = new StringBuilder();

			for (int i = 0; i < encParams.Length; i++)
			{
				paramInfo.Append("Param " + i + " holds " + encParams[i].NumberOfValues +
					" items of type " +
					encParams[i].ValueType + "\r\n" + "Guid category: " + encParams[i].Encoder.Guid + "\r\n");

			}

			//textBox1.Text = paramInfo.ToString();
		}
		*/
	}
}
