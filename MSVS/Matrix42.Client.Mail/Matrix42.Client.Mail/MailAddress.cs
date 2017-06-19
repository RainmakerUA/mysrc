
using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;

namespace Matrix42.Client.Mail
{
    public class MailAddress
    {
	    public MailAddress(string address, string displayName)
	    {
		    Address = address;
		    DisplayName = displayName;
	    }

		public string Address { get; }

		public string DisplayName { get; }

	    public override string ToString()
	    {
		    return (!String.IsNullOrEmpty(DisplayName) ? $"\"{DisplayName}\" " : String.Empty) + Address;
	    }

	    public static MailAddress From(MailboxAddress address)
	    {
		    return address != null ? new MailAddress(address.Address, address.Name) : null;
	    }

	    public static MailAddress From(InternetAddressList addresses)
	    {
		    return From(addresses.Mailboxes.FirstOrDefault());
	    }

	    public static IList<MailAddress> ListFrom(InternetAddressList addresses)
	    {
		    return addresses.Mailboxes.Select(From).ToArray();
	    }
    }
}
