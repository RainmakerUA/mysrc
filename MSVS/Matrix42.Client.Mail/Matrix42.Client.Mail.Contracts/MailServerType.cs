using System;

namespace Matrix42.Client.Mail.Contracts
{
	[Serializable]
	public enum MailServerType
	{
		Unknown = 0,

		Imap4 = 1,

		ExchangeWebDav = 2,

		Exchange2007 = 3,

		Exchange2007Sp1 = 4,

		Exchange2007Sp2 = 5,

		Exchange2007Sp3 = 6,

		Exchange2010 = 7,

		Exchange2010Sp1 = 8,

		Exchange2010Sp2 = 9
	}
}
