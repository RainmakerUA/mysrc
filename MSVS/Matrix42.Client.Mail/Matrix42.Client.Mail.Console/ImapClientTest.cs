using System;

namespace Matrix42.Client.Mail.Console
{
	internal class ImapClientTest
	{
		private readonly string _host;
		private readonly int? _port;
		private readonly bool _useSsl;
		private readonly string _username;
		private readonly string _password;
		private readonly string _folder;
		private readonly string _folderToMove;

		public ImapClientTest()
			: this("imap.gmail.com", null, true, "update4u.info", "update4u.", "Inbox/Incident/VZH test", "Inbox/Incident/VZH processed")
		{
			// Do nothing
		}

		public ImapClientTest(string host, int? port, bool useSsl, string username, string password, string folder, string folderToMove)
		{
			_host = host;
			_port = port;
			_useSsl = useSsl;
			_username = username;
			_password = password;
			_folder = folder;
			_folderToMove = folderToMove;
		}

		public void Execute()
		{
			try
			{
				var config = new ClientConfig(_host, _port, _useSsl, _username, _password, _folder, _folderToMove);

				using (var client = MailClientFactory.GetClient(config))
				{
					//FetchAndProcessMessage(client);
					//SaveMessage(client);
					//ListFolders(client);

					SearchMessages(client);
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
