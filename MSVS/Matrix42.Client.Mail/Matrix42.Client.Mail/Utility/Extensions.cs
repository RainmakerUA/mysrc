using System.Collections.Generic;
using System.Linq;
using Matrix42.Client.Mail.Contracts;
using Microsoft.Exchange.WebServices.Data;
using MimeKit;

namespace Matrix42.Client.Mail.Utility
{
	internal static class Extensions
	{
		public static MailAddress ToMailAddress(this EmailAddress address)
		{
			return address != null ? new MailAddress { Address = address.Address, DisplayName = address.Name } : null;
		}

		public static MailAddress ToMailAddress(this MailboxAddress address)
		{
			return address != null ? new MailAddress { Address = address.Address, DisplayName = address.Name } : null;
		}

		public static MailAddress ToMailAddress(this InternetAddressList addresses)
		{
			return ToMailAddress(addresses.Mailboxes.FirstOrDefault());
		}

		public static IList<MailAddress> ToMailAddresses(this EmailAddressCollection addresses)
		{
			return addresses?.Select(ToMailAddress).ToArray();
		}

		public static IList<MailAddress> ToMailAddresses(this InternetAddressList addresses)
		{
			return addresses?.Mailboxes.Select(ToMailAddress).ToArray();
		}
	}
}
