using System.Net;
using System.Net.Mail;

namespace Matrix42.Client.Mail.Console
{
	internal class SmtpClientTest
	{
		public void Execute()
		{
			using (var client = new SmtpClient("smtp.gmail.com"))
			{
				client.Credentials = new NetworkCredential("update4u.info", "update4u.");
				client.EnableSsl = true;

				using (var message = new MailMessage(
													"update4u.info@gmail.com", "vasiliy.zhusma@matrix42.com",
													"Test SmtpClient via TLS", "If you've got this message, then the test is passed.\r\nRegards, RM (VZH)"
												))
				{
					client.Send(message);
					System.Console.WriteLine($"Message is sent via: {client.ServicePoint.Address}");
				}
			}
		}
	}
}
