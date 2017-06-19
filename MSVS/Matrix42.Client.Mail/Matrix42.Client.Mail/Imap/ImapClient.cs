using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using MailKit;
using MailKit.Search;
using Matrix42.Client.Mail.Utility;
using MimeKit;
using KitImapClient = MailKit.Net.Imap.ImapClient;

namespace Matrix42.Client.Mail.Imap
{
	internal sealed class ImapClient : IMailClient
	{
		private const char _defaultSeparator = '/';

		private readonly ClientConfig _config;
		private readonly KitImapClient _client;

		private IMailFolder _folder;
		private IMailFolder _folderToMove;

		private bool _isDisposed;

		public ImapClient(ClientConfig config)
		{
			_config = config;
			_client = new KitImapClient();
			_isDisposed = false;
		}

		#region IMailClient members

		public ClientConfig Connection => _config;

		public string Folder => _config.Folder.Name;

		public string FolderToMove => _config.FolderToMove?.Name;

		public IList<string> GetUnreadMails()
		{
			return GetMails(true);
		}

		public IList<string> GetAllMails()
		{
			return GetMails(false);
		}

		public IList<string> SearchMails(string[] terms)
		{
			EnsureInitialized(FolderAccess.ReadOnly, false);

			if (terms.Length > 0)
			{
				SearchQuery subjectQuery = SearchQuery.SubjectContains(terms[0]);
				SearchQuery bodyQuery = SearchQuery.BodyContains(terms[0]);

				for (int i = 1; i < terms.Length; i++)
				{
					subjectQuery = SearchQuery.Or(subjectQuery, SearchQuery.SubjectContains(terms[i]));
					bodyQuery = SearchQuery.Or(bodyQuery, SearchQuery.SubjectContains(terms[i]));
				}

				var query = SearchQuery.Or(subjectQuery, bodyQuery);
				var results = _folder.Search(SearchOptions.All, query);

				return results.UniqueIds.Select(id => id.Id.ToString()).ToList().AsReadOnly();
			}

			return new string[0];
		}

		public IMessage GetMessage(string id)
		{
			return ImapMessage.FromMessage(GetMimeMessage(id));
		}

		public void MarkMessagesAsRead(params string[] ids)
		{
			EnsureInitialized(FolderAccess.ReadWrite, false);

			_folder.AddFlags(Array.ConvertAll(ids, UniqueId.Parse), MessageFlags.Seen, true);
		}

		public void MoveMessages(params string[] ids)
		{
			EnsureInitialized(FolderAccess.ReadWrite, true);

			_folder.MoveTo(Array.ConvertAll(ids, UniqueId.Parse), _folderToMove);
		}

		public IList<string> GetFolderNames(FolderType type)
		{
			EnsureInitialized(FolderAccess.None, true);

			var folders = _client.GetFolders(new FolderNamespace(_defaultSeparator, String.Empty));

			return folders.Select(f => f.FullName).ToList().AsReadOnly();
		}

		public IMessage LoadMessage(string fileName, string id)
		{
			return ImapMessage.FromMessage(FileHelper.LoadMimeMessage(fileName, id, out string _));
		}

		public void SaveMessage(string fileName, string id)
		{
			var msg = GetMimeMessage(id);
			FileHelper.SaveMimeMessage(msg, fileName, id);
		}

		public void Dispose()
		{
			try
			{
				SafeCloseFolder(_folder);
				SafeCloseFolder(_folderToMove);

				// ...?

				if (_client != null)
				{
					if (_client.IsConnected)
					{
						_client.Disconnect(true);
					}

					_client.Dispose();
				}
			}
			finally
			{
				_isDisposed = true;
			}
		}

		#endregion

		private void EnsureInitialized(FolderAccess folderAccess, bool withFolderToMove)
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException(nameof(ImapClient), $"{GetType().FullName} instance has been disposed");
			}

			if (_config == null)
			{
				throw new InvalidOperationException("IMAP Client is not configured!");
			}

			if (_client.ServerCertificateValidationCallback == null)
			{
				_client.ServerCertificateValidationCallback = NetworkHelper.ServerCertificateValidationCallback;
			}

			if (!_client.IsConnected)
			{
				_client.Connect(_config.Host, _config.Port.GetValueOrDefault(), _config.UseSsl);
			}

			if (!_client.IsAuthenticated)
			{
				_client.Authenticate(_config.Username, _config.Password);
			}

			if (_folder == null && folderAccess > FolderAccess.None)
			{
				_folder = _client.GetFolder(_config.Folder.Name);
			}

			if (_folder != null && (!_folder.IsOpen || _folder.Access < folderAccess))
			{
				_folder.Open(folderAccess);
			}

			if (withFolderToMove)
			{
				var folderToMove = _config.FolderToMove.Name;

				if (_folderToMove == null && !String.IsNullOrEmpty(folderToMove))
				{
					_folderToMove = _client.GetFolder(folderToMove);
				}

				// Destination folder doesn't need to be Opened (this leads to closing source folder)
			}
		}

		private string[] GetMails(bool unreadOnly)
		{
			EnsureInitialized(FolderAccess.ReadOnly, false);

			var uids = _folder.Search(unreadOnly ? SearchQuery.NotSeen : SearchQuery.All);
			return uids.Select(uid => uid.Id.ToString()).ToArray();
		}

		private MimeMessage GetMimeMessage(string id)
		{
			EnsureInitialized(FolderAccess.ReadOnly, false);
			
			return _folder.GetMessage(UniqueId.Parse(id));
		}

		private static void SafeCloseFolder(IMailFolder folder)
		{
			if (folder != null && folder.IsOpen)
			{
				folder.Close(folder.Access == FolderAccess.ReadWrite);
			}
		}
	}
}
