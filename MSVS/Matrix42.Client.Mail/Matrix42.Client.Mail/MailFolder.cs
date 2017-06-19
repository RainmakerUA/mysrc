
namespace Matrix42.Client.Mail
{
	public class MailFolder
	{
		public MailFolder(string name) : this(name, FolderType.Message)
		{
		}

		public MailFolder(string name, FolderType type)
		{
			Name = name;
			Type = type;
		}

		public string Name { get; }

		public FolderType Type { get; }

		public static implicit operator MailFolder(string name) => new MailFolder(name);

		public static explicit operator string(MailFolder folder) => folder.Name;
	}
}
