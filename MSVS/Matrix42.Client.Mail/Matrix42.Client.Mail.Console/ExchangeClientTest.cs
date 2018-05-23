using System;
using Matrix42.Client.Mail.Contracts;

namespace Matrix42.Client.Mail.Console
{
	internal class ExchangeClientTest
	{
		private readonly ClientConfig _config;

		public ExchangeClientTest(ClientConfig config)
		{
			_config = config;
		}

		public void Execute()
		{
			try
			{
				using (var client = MailClientFactory.GetClient(_config))
				{
					//FetchAndProcessMessage(client);
					//SaveMessage(client);
					ListFolders(client, FolderType.Public);

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

		private static void ListFolders(IMailClient client, FolderType folderType)
		{
			var folders = client.GetFolderNames(folderType);

			System.Console.WriteLine(folders.Count > 0 ? "{0} folders found:" : "No {0} folders found.", folderType);

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
