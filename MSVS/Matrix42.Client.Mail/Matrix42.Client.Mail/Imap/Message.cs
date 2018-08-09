using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Matrix42.Client.Mail.Contracts;
using Matrix42.Client.Mail.Utility;
using MimeKit;

namespace Matrix42.Client.Mail.Imap
{
	internal sealed class Message : IMessage, IWriteable
	{
		private readonly MimeMessage _message;

		public Message(MimeMessage message, string id)
		{
			_message = message;

			ID = id;
			From = message.From.ToMailAddress();
			Sender = message.Sender.ToMailAddress();
			To = message.To.ToMailAddresses();
			Cc = message.Cc.ToMailAddresses();
			Bcc = message.Bcc.ToMailAddresses();
			Subject = message.Subject;

			var alternativeSection = GetAlternativeSectionOfPhantomSectionMessage(message);

			if (alternativeSection != null)
			{
				(BodyText, BodyHtml) = (alternativeSection.TextBody, alternativeSection.HtmlBody);
			}
			else
			{
				BodyText = message.TextBody;
				BodyHtml = message.HtmlBody; //TODO: Handle text-only & html-only messages
            }
			Attachments = Attachment.ListFrom(message.BodyParts);
			ReceivedDate = message.Date != DateTimeOffset.MinValue ? message.Date.ToUniversalTime().DateTime : new DateTime?();
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

		public static Message FromMessage(MimeMessage message, string id)
		{
			return new Message(message, id);
		}

		private static Importance? ConvertImportance(MessageImportance importance)
		{
			switch (importance)
			{
				case MessageImportance.Low:
					return Contracts.Importance.Low;

				case MessageImportance.Normal:
					return Contracts.Importance.Normal;

				case MessageImportance.High:
					return Contracts.Importance.High;

				default:
					return new Importance?();
			}
		}

		private static MultipartAlternative GetAlternativeSectionOfPhantomSectionMessage(MimeMessage message)
		{
			return (message.Body as Multipart)?.OfType<MultipartAlternative>()
						.FirstOrDefault(
								ma => !String.IsNullOrEmpty(ma.HtmlBody) && !String.IsNullOrEmpty(ma.TextBody)
							);
		}
	}
}
