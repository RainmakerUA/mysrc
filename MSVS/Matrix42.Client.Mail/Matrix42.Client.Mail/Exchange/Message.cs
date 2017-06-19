using System;
using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;

namespace Matrix42.Client.Mail.Exchange
{
	internal sealed class Message : IMessage
	{
		private readonly EmailMessage _message;

		public Message(EmailMessage message, string id)
		{
			_message = message;

			ID = id;
			From = MailAddress.From(message.From);
			Sender = MailAddress.From(message.Sender);
			To = MailAddress.ListFrom(message.ToRecipients);
			Cc = MailAddress.ListFrom(message.CcRecipients);
			Bcc = MailAddress.ListFrom(message.BccRecipients);
			Subject = message.Subject;
			BodyText = message.TextBody;
			BodyHtml = message.Body; //TODO: Handle text-only & html-only messages
			Attachments = Attachment.ListFrom(message.Attachments);
			ReceivedDate = message.DateTimeReceived;
			Importance = ConvertImportance(message.Importance);
			OutOfOfficeReply = message.ItemClass == ExchangeConstants.OutOfOfficeMessageClass;
		}

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

		public static Message FromMessage(EmailMessage message, string id)
		{
			return new Message(message, id);
		}

		private static Importance? ConvertImportance(Microsoft.Exchange.WebServices.Data.Importance importance)
		{
			switch (importance)
			{
				case Microsoft.Exchange.WebServices.Data.Importance.Low:
					return Mail.Importance.Low;

				case Microsoft.Exchange.WebServices.Data.Importance.Normal:
					return Mail.Importance.Normal;

				case Microsoft.Exchange.WebServices.Data.Importance.High:
					return Mail.Importance.High;

				default:
					return new Importance?();
			}
		}
	}
}
