using System;
using System.Text.RegularExpressions;
using Matrix42.Client.Mail.Contracts;
using Con = System.Console;

namespace Matrix42.Client.Mail.Console
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var exchConfig = ClientConfig.MakeConfig(
												MailServerType.Exchange2010Sp2,
												"outlook.office.de",
												null, true,
												"andreykostenko@matrix42Office365GER.onmicrosoft.de",
												"andreykostenko@matrix42Office365GER.onmicrosoft.de", "Matrix42!",
												new MailFolder { Name = "Inbox/emr", Type = FolderType.Message },
												/*new MailFolder { Name = "Inbox/subin1", Type = FolderType.Message }*/null,
												false
											);
			var exchConfig2 = ClientConfig.MakeConfig(
												MailServerType.Exchange2010Sp2,
												"outlook.office.de",
												null, true,
												"matrix42Office365GER@matrix42Office365GER.onmicrosoft.de",
												"matrix42Office365GER@matrix42Office365GER.onmicrosoft.de", "8tzT8ErszHa0fO9K",
												new MailFolder { Name = "PF_XY", Type = FolderType.Public },
												/*new MailFolder { Name = "Inbox/subin1", Type = FolderType.Message }*/null,
												false
											);
			var imapExchConfig = ClientConfig.MakeConfig(
												MailServerType.Imap4, //MailServerType.Exchange2010Sp2,
												"outlook.office.de",
												null, true,
												"andreykostenko@matrix42Office365GER.onmicrosoft.de",
												"andreykostenko@matrix42Office365GER.onmicrosoft.de", "Matrix42!",
												new MailFolder { Name = "Inbox/subin1", Type = FolderType.Message },
												new MailFolder { Name = "Inbox/VZH/Störungen", Type = FolderType.Message },//*/null,
												false
											);
			var imapExch2Config = ClientConfig.MakeConfig(
												MailServerType.Imap4, //MailServerType.Exchange2010Sp2,
												"outlook.office.de",
												null, true,
												"matrix42Office365GER@matrix42Office365GER.onmicrosoft.de",
												"matrix42Office365GER@matrix42Office365GER.onmicrosoft.de", "8tzT8ErszHa0fO9K",
												new MailFolder { Name = "Inbox/subin1", Type = FolderType.Message },
												new MailFolder { Name = "Inbox/VZH/Störungen", Type = FolderType.Message },//*/null,
												false
											);

			//var parserTest = new ParserTest();
			//parserTest.Execute();

			var imapTest = new ImapClientTest(imapExch2Config); //new ExchangeClientTest(exchConfig2);
			imapTest.Execute();

			//var mimeTest = new MimeTest();
			//mimeTest.Execute();

			//var msgParserTest = new ParseMsgTest();
			//msgParserTest.Execute();

			//var test = new SmtpClientTest();
			//test.Execute();

			Con.Write("Press any key...");
			Con.ReadKey(true);
		}
	}
}
