using System;
using System.Collections.Generic;

namespace Matrix42.Client.Mail
{
    public interface IMessage
    {
		string ID { get; }

		MailAddress From { get; }

		MailAddress Sender { get; }

		IList<MailAddress> To { get; }

		IList<MailAddress> Cc { get; }

		IList<MailAddress> Bcc { get; }

		string Subject { get; }

		string BodyText { get; }

		string BodyHtml { get; }

		IList<IAttachment> Attachments { get; }

		DateTime? ReceivedDate { get; }

		Importance? Importance { get; }

		bool OutOfOfficeReply { get; }
    }
}
