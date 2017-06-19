﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix42.Client.Mail
{
	public static class MailClientFactory
	{
		public static IMailClient GetClient(ClientConfig config /*, client type*/)
		{
			return new Imap.ImapClient(config);
		}
	}
}