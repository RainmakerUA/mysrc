using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Exchange.WebServices.Data;

namespace Matrix42.Client.Mail.Exchange
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

		public static Attachment From(Microsoft.Exchange.WebServices.Data.Attachment attachment)
		{
			using (var ms = new MemoryStream())
			{
				byte[] data;

				switch (attachment)
				{
					case FileAttachment fAtt:
						fAtt.Load(ms);
						data = ms.ToArray();
						break;

					case ItemAttachment iAtt:
						iAtt.Load(ItemSchema.MimeContent);
						data = iAtt.Item.MimeContent.Content;
						break;

					default:
						data = Encoding.ASCII.GetBytes($"Attachment type {attachment.GetType().FullName} not supported!");
						break;
				}

				return new Attachment(attachment.Name, attachment.ContentId, attachment.ContentType, data);
			}
		}

		public static IList<IAttachment> ListFrom(AttachmentCollection attachments)
		{
			return attachments?.Select(From).ToArray<IAttachment>();
		}
	}
}
