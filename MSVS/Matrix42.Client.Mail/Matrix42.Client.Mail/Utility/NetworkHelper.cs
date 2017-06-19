using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Matrix42.Client.Mail.Utility
{
	internal static class NetworkHelper
	{
		public static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			//TODO: rewrite to allow only self-signed certificates
			return true;
		}
	}
}
