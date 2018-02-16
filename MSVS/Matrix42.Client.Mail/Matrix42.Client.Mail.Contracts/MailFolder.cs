using System;
using System.Runtime.Serialization;

namespace Matrix42.Client.Mail.Contracts
{
	[Serializable]
	[DataContract]
	public class MailFolder
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public FolderType Type { get; set; }
	}
}
