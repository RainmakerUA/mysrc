using System;
using System.Collections.Generic;
using System.IO;
using MimeKit;

namespace Matrix42.Client.Mail.Imap
{
	internal sealed class ImapMessage : IMessage, Utility.IWriteable
	{
		private readonly MimeMessage _message;

		public ImapMessage(MimeMessage message)
		{
			_message = message;

			ID = message.MessageId;
			From = MailAddress.From(message.From);
			Sender = MailAddress.From(message.Sender);
			To = MailAddress.ListFrom(message.To);
			Cc = MailAddress.ListFrom(message.Cc);
			Bcc = MailAddress.ListFrom(message.Bcc);
			Subject = message.Subject;
			BodyText = message.TextBody;
			BodyHtml = message.HtmlBody;
			Attachments = ImapAttachment.ListFrom(message.BodyParts);
			ReceivedDate = message.Date != DateTimeOffset.MinValue ? message.Date.DateTime : new DateTime?();
			Importance = ConvertImportance(message.Importance);
			OutOfOfficeReply = false; // TODO: Do we need it for IMAP?
		}

		#region IMessage members
		
		public string ID { get; }

		public MailAddress From { get; }

		public MailAddress Sender { get; }

		public IList<MailAddress> To { get; }

		public IList<MailAddress> Cc { get; }

		public IList<MailAddress> Bcc { get; }

		public string Subject { get; }

		public string BodyText { get; }

		public string BodyHtml { get; }

		public IList<IAttachment> Attachments { get; }

		public DateTime? ReceivedDate { get; }

		public Importance? Importance { get; }

		public bool OutOfOfficeReply { get; }

		#endregion

		public void WriteTo(Stream stream)
		{
			_message.WriteTo(stream);
		}

		public static ImapMessage FromMessage(MimeMessage message)
		{
			return new ImapMessage(message);
		}

		private static Importance? ConvertImportance(MessageImportance importance)
		{
			switch (importance)
			{
				case MessageImportance.Low:
					return Mail.Importance.Low;

				case MessageImportance.Normal:
					return Mail.Importance.Normal;

				case MessageImportance.High:
					return Mail.Importance.High;

				default:
					return new Importance?();
			}
		}
	}
}
