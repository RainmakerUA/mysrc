using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix42.Client.Mail.Contracts;
using Microsoft.Exchange.WebServices.Data;

namespace Matrix42.Client.Mail
{
	public static class MailClientFactory
	{
		public static IMailClient GetClient(ClientConfig config, bool msex  /*, client type*/)
		{
			return msex ? new Exchange.Client(config, ExchangeVersion.Exchange2016) : (IMailClient)new Imap.Client(config);
		}
	}
}
