using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Matrix42.Client.Mail.Contracts;

namespace Matrix42.Client.Mail.Utility
{
	internal static class NetworkHelper
	{
		public static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			// If the certificate is a valid, signed certificate, return true.
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				return true;
			}

			// If there are errors in the certificate chain, look at each error to determine the cause.
			if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
			{
				if (chain?.ChainStatus != null)
				{
					foreach (var status in chain.ChainStatus)
					{
						if ((certificate.Subject != certificate.Issuer || status.Status != X509ChainStatusFlags.UntrustedRoot)
								&& status.Status != X509ChainStatusFlags.NoError)
						{
							return false;
						}

						// Self-signed certificates with an untrusted root are valid.
					}
				}

				// When processing reaches this line, the only errors in the certificate chain are 
				// untrusted root errors for self-signed certificates. These certificates are valid
				// for default Exchange server installations, so return true.
				return true;
			}

			// In all other cases, return false.
			return false;
		}

		/// <summary>
		/// Detects valid mail address to be used for looking up mailbox folders.
		/// Not used for Public folders.
		/// </summary>
		/// <param name="config">Connection configuration.</param>
		/// <returns>Mail address or null when account name is an actual address.</returns>
		public static string GetMailAddress(ClientConfig config)
		{
			const char at = '@';
			var mailAddress = config.MailAddress;
			var accountName = config.Username;
			var accountNameAtPos = accountName.IndexOf(at);

			if (accountNameAtPos < 0)
			{
				// account name is not an address
				// it is correct to return 'null' here
				return mailAddress;
			}

			if (accountName.Equals(mailAddress, StringComparison.InvariantCultureIgnoreCase))
			{
				// account name is same as mail address
				// it is correct to return 'null' here
				return mailAddress;
			}

			var mailAddressServerName = mailAddress.Substring(mailAddress.IndexOf(at) + 1);
			var accountServerName = accountName.Substring(accountNameAtPos + 1);

			if (accountServerName.Equals(mailAddressServerName, StringComparison.InvariantCultureIgnoreCase))
			{
				// server names are the same, likely a shared folder is used
				return mailAddress;
			}

			return null;
		}

	}
}
