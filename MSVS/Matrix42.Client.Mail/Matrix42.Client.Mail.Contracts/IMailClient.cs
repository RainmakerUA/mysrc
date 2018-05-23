using System;
using System.Collections.Generic;

namespace Matrix42.Client.Mail.Contracts
{
    public interface IMailClient : IDisposable
    {
		/*int ServerType { get; }

		ClientConfig Connection { get; }

		string Folder { get; }

		string FolderToMove { get; }

		string FolderSeparator { get; }

		bool IgnoreAbsenceMails { get; }

		bool EnableLog { get; }

		int TicketType { get; }

		Guid Category { get; }

		Guid Template { get; }*/

	    IReadOnlyList<string> GetUnreadMails();

		IReadOnlyList<string> GetAllMails();

		IReadOnlyList<string> SearchMails(params string[] terms);

		IMessage GetMessage(string id);

	    void MarkMessagesAsRead(params string[] ids);

	    void MoveMessages(params string[] ids);

		IReadOnlyList<string> GetFolderNames(FolderType type);

	    IMessage LoadMessage(string fileName, string id);

		void SaveMessage(string fileName, string id);
    }
}
