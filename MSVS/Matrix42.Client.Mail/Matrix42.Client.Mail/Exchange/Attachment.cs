using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Matrix42.Client.Mail.Utility;
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
			return new Attachment(GetName(attachment), attachment.ContentId, attachment.ContentType, GetData(attachment));
		}

		public static IList<IAttachment> ListFrom(AttachmentCollection attachments)
		{
			return attachments?.Select(From).ToArray<IAttachment>();
		}

		private static string GetName(Microsoft.Exchange.WebServices.Data.Attachment attachment)
		{
			switch (attachment)
			{
				case FileAttachment fAtt:
					return FileHelper.MakeValidFileName(fAtt.FileName ?? fAtt.Name, fAtt.ContentType);

				case ItemAttachment iAtt:
					iAtt.Load(ItemSchema.Subject);
					/*new PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.TextBody, ItemSchema.Attachments, ItemSchema.MimeContent*/

					string subject = null;
					iAtt.Load(ItemSchema.Subject));

					try
					{
						subject = iAtt.Item.Subject;
					}
					catch (ServiceObjectPropertyException e)
					{
						Console.WriteLine(e); // TODO: Log?
					}

					return FileHelper.MakeValidFileName(subject, null) + FileHelper.EmlExtension;

				default:
					// return null;
					// return FileHelper.MakeValidFileName(null, part.ContentType.MimeType);
					throw new NotSupportedException($"{attachment.GetType().FullName} is not supported");
			}
		}

		private static byte[] GetData(Microsoft.Exchange.WebServices.Data.Attachment attachment)
		{
			switch (attachment)
			{
				case FileAttachment fAtt:
					using (var ms = new MemoryStream())
					{
						fAtt.Load(ms);
						return ms.ToArray();
					}

				case ItemAttachment iAtt:
					iAtt.Load(ItemSchema.MimeContent);
					return iAtt.Item.MimeContent.Content;

				default:
					return Encoding.ASCII.GetBytes($"Attachment type {attachment.GetType().FullName} not supported!");
			}
		}
	}
}
