using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Matrix42.Client.Mail.Contracts;
using MimeKit;

namespace Matrix42.Client.Mail.Utility
{
	internal static class FileHelper
	{
		private const char _invalidCharacter = '\uFFFD';
		private const string _emr = ".emr";
		private const string _eml = ".eml";
		private const string _dat = ".dat";
		private const string _newline = "\r\n";
		private const string _emrHeader = "\u0020*\u0020Matrix42 E-mail Robot Exported Message. Message ID\u0020=\u0020";
		private const string _datEmailSourceCode = "\u0020Email Source code:";
		private const int _datHeaderMaxLines = 8;
		private static readonly Regex _mailUidRegex = new Regex(@"MailUID=(\d+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex _fetchUidRegex = new Regex(@"(?:\(|\s)UID (\d+)(?:\s+|\)$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static string EmlExtension => _eml;

		public static MimeMessage LoadMimeMessage(string fileName, string id, out string idInFile)
		{
			using (var fs = File.OpenRead(fileName))
			{
				idInFile = ReadFileHeader(fs);
				return MimeMessage.Load(fs, false);
			}
		}

		public static void SaveMimeMessage(MimeMessage message, string fileName, string id)
		{
			SaveToFile(fileName, id, s => message.WriteTo(s));
		}

		public static void SaveMimeContent(byte[] mimeContent, string fileName, string id)
		{
			SaveToFile(fileName, id, s => s.Write(mimeContent, 0, mimeContent.Length));
		}

		/* TODO: Unused
		public static void SaveMessage(IMessage message, string fileName, string id)
		{
			if (message is IWriteable writeable)
			{
				if (String.IsNullOrEmpty(Path.GetExtension(fileName)))
				{
					fileName = Path.ChangeExtension(fileName, _emr);
				}

				var header = String.Concat(_emrHeader, id, _newline);
				var headerBytes = Encoding.ASCII.GetBytes(header);

				using (var fs = File.Create(fileName))
				{
					fs.Write(headerBytes, 0, headerBytes.Length);
					writeable.WriteTo(fs);
				}
			}
			else
			{
				// TODO: Don't throw, only log an error
				throw new NotSupportedException($"Cannot save message of type {message.GetType().FullName}");
			}
		}
		*/

		public static void ConvertMessageFile(string filename, string defaultID)
		{
			var msg = LoadMimeMessage(filename, defaultID, out string fileID);

			if (msg.Body is Multipart multipart)
			{
				multipart.Epilogue = null;
			}

			SaveMimeMessage(msg, Path.ChangeExtension(filename, _emr), fileID ?? defaultID);
		}

		public static string MakeValidFileName(string filename, string mimeType)
		{
			var invalidChars = Path.GetInvalidFileNameChars();
			var sb = new StringBuilder(filename);

			for (int i = 0; i < sb.Length; i++)
			{
				if (sb[i].Equals(_invalidCharacter) || Array.IndexOf(invalidChars, sb[i]) >= 0)
				{
					sb[i] = '_';
				}
			}

			var correctedName = sb.ToString();

			if (String.IsNullOrEmpty(Path.GetExtension(correctedName)) && !String.IsNullOrEmpty(mimeType))
			{
				correctedName = Path.ChangeExtension(correctedName, ResolveFileExtension(mimeType));
			}

			return correctedName;
		}

		private static string ReadFileHeader(FileStream fs)
		{
			var extension = Path.GetExtension(fs.Name);
			bool isDat;

			switch (extension)
			{
				case _eml:
					// Do nothing. Go directly to parsing message
					return null;

				case _dat:
					isDat = true;
					break;

				default:
					// Treat all other extensions (together with .emr) as EMR format
					isDat = false;
					break;
			}

			return ReadMessageID(fs, isDat);
		}

		private static void SaveToFile(string fileName, string id, Action<Stream> writeContent)
		{
			if (String.IsNullOrEmpty(Path.GetExtension(fileName)))
			{
				fileName = Path.ChangeExtension(fileName, _emr);
			}

			var header = String.Concat(_emrHeader, id, _newline);
			var headerBytes = Encoding.ASCII.GetBytes(header);

			using (var fs = File.Create(fileName))
			{
				fs.Write(headerBytes, 0, headerBytes.Length);
				writeContent(fs);
			}
		}

		private static string ReadMessageID(Stream fs, bool isDat)
		{
			using (var reader = new StreamLineReader(fs))
			{
				var header = reader.ReadLine();

				if (isDat)
				{
					string uid = null;
					int linesLeft = _datHeaderMaxLines - 1;

					while (header != null && linesLeft > 0)
					{
						if (uid == null)
						{
							var matches = _mailUidRegex.Matches(header);
							if (matches.Count > 0)
							{
								uid = matches[0].Groups[1].Value;
							}
							else
							{
								matches = _fetchUidRegex.Matches(header);

								if (matches.Count > 0)
								{
									uid = matches[0].Groups[1].Value;
								}
							}
						}

						if (header.Equals(_datEmailSourceCode, StringComparison.Ordinal))
						{
							linesLeft = 1;
						}

						linesLeft -= 1;
						header = reader.ReadLine();
					}

					return uid;
				}

				return !String.IsNullOrEmpty(header) && header.StartsWith(_emrHeader, StringComparison.Ordinal)
							? header.Substring(_emrHeader.Length)
							: null;
			}
		}

		private static string ResolveFileExtension(string mimeType)
		{
			// TODO?: MIME type detection by data

			var extensions = MimeTypeHelper.GetExtensions(mimeType);
			return extensions.FirstOrDefault() ?? String.Empty;
		}
	}
}
