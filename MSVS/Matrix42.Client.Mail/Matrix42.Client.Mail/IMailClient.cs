using System;
using System.Collections.Generic;

namespace Matrix42.Client.Mail
{
    public interface IMailClient : IDisposable
    {
		//int ServerType { get; }

		ClientConfig Connection { get; }

		string Folder { get; }

		string FolderToMove { get; }

		/*string FolderSeparator { get; }

		bool IgnoreAbsenceMails { get; }

		bool EnableLog { get; }

		int TicketType { get; }

		Guid Category { get; }

		Guid Template { get; }*/

	    IList<string> GetUnreadMails();

	    IList<string> GetAllMails();

	    IList<string> SearchMails(string[] terms);

		IMessage GetMessage(string id);

	    void MarkMessagesAsRead(string[] ids);

	    void MoveMessages(string[] ids);

	    IList<string> GetFolderNames(FolderType type);

	    IMessage LoadMessage(string fileName, string id);

		void SaveMessage(string fileName, string id);
    }
}
