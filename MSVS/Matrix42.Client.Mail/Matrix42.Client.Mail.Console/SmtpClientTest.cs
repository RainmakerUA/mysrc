using System.Net;
//using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Matrix42.Client.Mail.Console
{
	internal class SmtpClientTest
	{
		public void Execute()
		{
			System.Console.Write("Gimme file to send:");
			var filename = System.Console.ReadLine();
			var message = MimeMessage.Load(ParserOptions.Default, filename);

			//message.To = new InternetAddressList(new []{ new MailboxAddress("matrix42Office365GER@matrix42Office365GER.onmicrosoft.de") });

			using (var client = new SmtpClient())
			{
				client.Connect(/*"smtp.gmail.com"*/"smtp.rambler.ru", 465, SecureSocketOptions.SslOnConnect/**/);
				var credentials = new NetworkCredential("raintrash", "<todo:password>"/*"update4u.info", "update4u."*/);
				client.Authenticate(credentials);

				var sender = new MailboxAddress("update4u.info@gmail.com");
				client.Send(message/*, sender, new [] { new MailboxAddress("matrix42Office365GER@matrix42Office365GER.onmicrosoft.de") }*/);
				System.Console.WriteLine($"Message is sent via: {client}");
			}
		}
	}
}
