using System;
using Matrix42.Client.Mail.Contracts;

namespace Matrix42.Client.Mail
{
	public static class MailClientFactory
	{
		public static IMailClient GetClient(ClientConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			return GetClient(config.ServerType, config);
		}

		public static IMailClient GetClient(MailServerType type)
		{
			return GetClient(type, null);
		}

		public static IMailClient GetClient(MailServerType type, ClientConfig config)
		{
			switch (type)
			{
				case MailServerType.Exchange2007:
				case MailServerType.Exchange2007Sp1:
				case MailServerType.Exchange2007Sp2:
				case MailServerType.Exchange2007Sp3:
				case MailServerType.Exchange2010:
				case MailServerType.Exchange2010Sp1:
				case MailServerType.Exchange2010Sp2:
					return new Exchange.Client(config);

				case MailServerType.ExchangeWebDav:
				case MailServerType.Imap4:
					return new Imap.Client(config);

				default:
					throw new NotSupportedException($"Client type {config.ServerType} is not supported!");
			}
		}
	}
}
