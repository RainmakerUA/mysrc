using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Matrix42.Client.Mail.Contracts;
using Matrix42.Client.Mail.Utility;
using MimeKit;

namespace Matrix42.Client.Mail.Imap
{
	internal sealed class Attachment : IAttachment
	{
		public Attachment(string name, string cid, string mimeType, byte[] data)
		{
			Name = name;
			Cid = cid;
			MimeType = mimeType;
			Data = data;
		}

		public string Name { get; }

		public string Cid { get; }

		public string MimeType { get; }

		public byte[] Data { get; }

		public static Attachment From(MimeEntity entity)
		{
			return new Attachment(GetName(entity), entity.ContentId, entity.ContentType.MimeType, GetData(entity));
		}

		public static IList<IAttachment> ListFrom(IEnumerable<MimeEntity> parts)
		{
			return parts?.Where(IsAttachment).Select(From).ToArray<IAttachment>();
		}

		public static bool IsAttachment(MimeEntity entity)
		{
			var disposition = entity.ContentDisposition?.Disposition;
			return disposition != null
					&& (
						disposition.Equals("attachment", StringComparison.OrdinalIgnoreCase)
							|| disposition.Equals("inline", StringComparison.OrdinalIgnoreCase)
								&& (!String.IsNullOrEmpty(entity.ContentId) || entity is MimePart part && !String.IsNullOrEmpty(part.FileName))
					);
		}

		private static string GetName(MimeEntity entity)
		{
			const string nonameFile = "File";

			if (entity is MessagePart message)
			{
				return FileHelper.MakeValidFileName(message.Message.Subject, null) + FileHelper.EmlExtension;
			}

			if (entity is MimePart part)
			{
				return FileHelper.MakeValidFileName(!String.IsNullOrEmpty(part.FileName) ? part.FileName : nonameFile, part.ContentType.MimeType);
			}

			return nonameFile;
		}

		private static byte[] GetData(MimeEntity entity)
		{
			using (var stream = new MemoryStream())
			{
				if (entity is MessagePart message)
				{
					message.Message.WriteTo(stream);
				}
				else if (entity is MimePart part)
				{
					part.Content.DecodeTo(stream);
				}
				else
				{
					var msg = Encoding.ASCII.GetBytes($"[!] Cannot parse section of type {entity.GetType().FullName}!");
					stream.Write(msg, 0, msg.Length);
				}

				return stream.ToArray();
			}
		}
	}
}
