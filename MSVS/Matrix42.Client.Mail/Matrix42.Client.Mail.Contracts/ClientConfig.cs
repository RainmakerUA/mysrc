using System;
using System.Runtime.Serialization;

namespace Matrix42.Client.Mail.Contracts
{
	[Serializable]
	[DataContract]
	public sealed class ClientConfig
	{
		[DataMember]
		public MailServerType ServerType { get; set; }

		[DataMember]
		public string Host { get; set; }

		[DataMember]
		public int? Port { get; set; }

		[DataMember]
		public bool UseSsl { get; set; }

		[DataMember]
		public string MailAddress { get; set; }

		[DataMember]
		public string Username { get; set; }

		[DataMember]
		public string Password { get; set; }

		[DataMember]
		public MailFolder Folder { get; set; }

		[DataMember]
		public MailFolder FolderToMove { get; set; }

		[DataMember]
		public bool IgnoreAbsenceEmails { get; set; }

		public static ClientConfig MakeConfig(MailServerType serverType, string host, int? port, bool useSsl, string mailAddress, string username, string password,
												MailFolder folder, MailFolder folderToMove, bool ignoreAbsenceEmails = false)
		{
			return new ClientConfig
						{
							ServerType = serverType,
							Host = host,
							Port = port,
							UseSsl = useSsl,
							Username = username,
							Password = password,
							Folder = folder,
							FolderToMove = folderToMove,
							MailAddress = mailAddress,
							IgnoreAbsenceEmails = ignoreAbsenceEmails
						};
		}
	}
}
