using System;
using System.Collections.Generic;
using Matrix42.Client.Mail.Contracts;
using Matrix42.Client.Mail.Utility;
using Microsoft.Exchange.WebServices.Data;
using Importance = Matrix42.Client.Mail.Contracts.Importance;

namespace Matrix42.Client.Mail.Exchange
{
	internal sealed class Message : IMessage
	{
		//private readonly EmailMessage _message;

		public Message(EmailMessage message, string id)
		{
			//_message = message;

			ID = id;
			From = message.From.ToMailAddress();
			Sender = message.Sender.ToMailAddress();
			To = message.ToRecipients.ToMailAddresses();
			Cc = message.CcRecipients.ToMailAddresses();
			Bcc = message.BccRecipients.ToMailAddresses();
			Subject = message.Subject;
			BodyText = message.TextBody;
			BodyHtml = message.Body; //TODO: Handle text-only & html-only messages
			Attachments = Attachment.ListFrom(message.Attachments);
			ReceivedDate = message.DateTimeReceived;
			Importance = ConvertImportance(message.Importance);
			OutOfOfficeReply = message.ItemClass == Constants.OutOfOfficeMessageClass;
		}

		public Message(PostItem message, string id)
		{
			//_message = message;

			ID = id;
			From = message.From.ToMailAddress();
			Sender = message.Sender.ToMailAddress();
			//To = message.ToRecipients.ToMailAddresses();
			//Cc = message.CcRecipients.ToMailAddresses();
			//Bcc = message.BccRecipients.ToMailAddresses();
			Subject = message.Subject;
			BodyText = message.TextBody;
			BodyHtml = message.Body; //TODO: Handle text-only & html-only messages
			Attachments = Attachment.ListFrom(message.Attachments);
			ReceivedDate = message.DateTimeReceived;
			Importance = ConvertImportance(message.Importance);
			OutOfOfficeReply = message.ItemClass == Constants.OutOfOfficeMessageClass;
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

		public static Message FromItem(Item item, string id)
		{
			switch (item)
			{
				case EmailMessage message:
					return new Message(message, id);

				case PostItem postItem:
					return new Message(postItem, id);

				default:
					throw new NotSupportedException($"Item type '{item.GetType().Name}' is not supported!");
			}
		}

		private static Importance? ConvertImportance(Microsoft.Exchange.WebServices.Data.Importance importance)
		{
			switch (importance)
			{
				case Microsoft.Exchange.WebServices.Data.Importance.Low:
					return Contracts.Importance.Low;

				case Microsoft.Exchange.WebServices.Data.Importance.Normal:
					return Contracts.Importance.Normal;

				case Microsoft.Exchange.WebServices.Data.Importance.High:
					return Contracts.Importance.High;

				default:
					return new Importance?();
			}
		}
	}
}
