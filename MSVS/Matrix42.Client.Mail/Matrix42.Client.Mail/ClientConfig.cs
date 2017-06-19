
namespace Matrix42.Client.Mail
{
	public sealed class ClientConfig
	{
		public ClientConfig(string host, int? port, bool useSsl, string mailAddress, string username, string password,
								MailFolder folder, MailFolder folderToMove, bool ignoreAbsenceEmails = false)
		{
			Host = host;
			Port = port;
			UseSsl = useSsl;
			Username = username;
			Password = password;
			Folder = folder;
			FolderToMove = folderToMove;
			MailAddress = mailAddress;
			IgnoreAbsenceEmails = ignoreAbsenceEmails;
		}

		public string Host { get; }

		public int? Port { get; }

		public bool UseSsl { get; }

		public string MailAddress { get; }

		public string Username { get; }

		public string Password { get; }

		public MailFolder Folder { get; }

		public MailFolder FolderToMove { get; }

		public bool IgnoreAbsenceEmails { get; }
	}
}
