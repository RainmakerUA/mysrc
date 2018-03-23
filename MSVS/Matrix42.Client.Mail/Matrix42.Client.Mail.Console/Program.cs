using System.IO;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Con = System.Console;

namespace Matrix42.Client.Mail.Console
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			//var parserTest = new ParserTest();
			//parserTest.Execute();

			var imapTest = new ExchangeClientTest();
			imapTest.Execute();

			//var mimeTest = new MimeTest();
			//mimeTest.Execute();

			//var msgParserTest = new ParseMsgTest();
			//msgParserTest.Execute();

			//Con.Write("Gimme HTML file: ");

			//var path = Con.ReadLine()?.Trim();
			//SendTest(path);

			Con.Write("Press any key...");
			Con.ReadKey(true);
		}

		private static void SendTest(string htmlPath)
		{
			var msg = new MimeMessage
						{
							Subject = "Bold spaces are weirdo!",
							Body = new TextPart(TextFormat.Html){ Text = File.ReadAllText(htmlPath) }
						};
			msg.From.Add(new MailboxAddress("Update4u Mail", "update4u.info@gmail.com"));
			msg.To.Add(new MailboxAddress("AK = You", "andreykostenko@matrix42Office365GER.onmicrosoft.de"));

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.gmail.com", 587);
				client.Authenticate("update4u.info@gmail.com", "update4u.");
				client.Send(msg);
				client.Disconnect(true);
			}
		}
	}
}
