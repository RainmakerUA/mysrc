using System;

namespace Matrix42.Client.Mail.Console
{
	internal class ExchangeClientTest
	{
		private readonly string _host;
		private readonly int? _port;
		private readonly bool _useSsl;
		private readonly string _username;
		private readonly string _password;
		private readonly string _mailAddress;
		private readonly string _folder;
		private readonly string _folderToMove;
		private readonly bool _ignoreOoo;

		public ExchangeClientTest()
			: this("outlook.office.de", null, true, "matrix42Office365GER@matrix42Office365GER.onmicrosoft.de", "8tzT8ErszHa0fO9K",
					"matrix42Office365GER@matrix42Office365GER.onmicrosoft.de", "ng-test", null, false)
		{
			// Do nothing
		}

		public ExchangeClientTest(string host, int? port, bool useSsl, string username, string password, string mailAddress, string folder, string folderToMove, bool ignoreOoo)
		{
			_host = host;
			_port = port;
			_useSsl = useSsl;
			_username = username;
			_password = password;
			_mailAddress = mailAddress;
			_folder = folder;
			_folderToMove = folderToMove;
			_ignoreOoo = ignoreOoo;
		}

		public void Execute()
		{
			try
			{
				var config = new ClientConfig(_host, _port, _useSsl, _mailAddress, _username, _password, _folder, _folderToMove, _ignoreOoo);

				using (var client = MailClientFactory.GetClient(config, true))
				{
					FetchAndProcessMessage(client);
					//SaveMessage(client);
					//ListFolders(client);

					//SearchMessages(client);
				}
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
			}
		}

		private static void FetchAndProcessMessage(IMailClient client)
		{
			var ids = client.GetUnreadMails();
			var len = ids.Count;
			var id = ids[len - 1];

			var idarr = new[] { id };
			var msg = len > 0 ? client.GetMessage(id) : null;
			ProcessMessage(msg);
			client.MarkMessagesAsRead(idarr);
			client.MoveMessages(idarr);
		}

		private static void SaveMessage(IMailClient client)
		{
			var ids = client.GetUnreadMails();
			var len = ids.Count;
			var id = ids[len - 1];

			client.SaveMessage(@"E:\_Temp\out.emr", id);
		}

		private static void ListFolders(IMailClient client)
		{
			var folders = client.GetFolderNames(FolderType.Message);

			System.Console.WriteLine(folders.Count > 0 ? "Folders found:" : "No folders found.");

			foreach (var folder in folders)
			{
				System.Console.WriteLine(folder);
			}
		}

		private static void SearchMessages(IMailClient client)
		{
			var ids = client.SearchMails(new[] {"statistics", "problemo"});

			System.Console.WriteLine(ids.Count > 0 ? "Messages found (IDs):" : "No messages found.");
			System.Console.WriteLine(String.Join(",\u0020", ids));
		}

		private static void ProcessMessage(IMessage msg)
		{
			System.Console.WriteLine("Got message: '{0}' from {1}", msg.Subject, msg.From);
		}
	}
}
