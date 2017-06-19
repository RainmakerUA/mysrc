using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using Matrix42.Client.Mail.Utility;
using Microsoft.Exchange.WebServices.Data;

namespace Matrix42.Client.Mail.Exchange
{
	internal sealed class Client : IMailClient
	{
		private const string _serviceUrlFormat = "{0}://{1}/EWS/Exchange.asmx";
		private const string _folderSeparator = @"\";
		private const int _messageMaxCount = 128;
		private const int _subfolderLimit = 100;

		private readonly ClientConfig _config;
		private readonly ExchangeVersion _serverVersion; //TODO: Replace with GSS type.

		private ExchangeService _service;
		private FolderId _folderID;
		private FolderId _folderToMoveID;
		private RemoteCertificateValidationCallback _oldGlobalCertificateValidationCallback;

		public Client(ClientConfig config, ExchangeVersion serverVersion)
		{
			_config = config;
			_serverVersion = serverVersion;
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
			if (terms.Length > 0)
			{
				var filter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or);

				foreach (var term in terms)
				{
					filter.Add(new SearchFilter.ContainsSubstring(ItemSchema.Subject, term));
					filter.Add(new SearchFilter.ContainsSubstring(ItemSchema.TextBody, term));
				}

				return SearchMailIDs(filter);
			}

			return new string[0];
		}

		public IMessage GetMessage(string id)
		{
			EnsureInitialized(false);

			// var properties = new PropertySet(BasePropertySet.IdOnly, ItemSchema.MimeContent, ItemSchema.TextBody, ItemSchema.Attachments);

			var properties = new PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.TextBody, ItemSchema.Attachments);

			var email = EmailMessage.Bind(_service, new ItemId(id), properties);

			return Message.FromMessage(email, id);

			//using (var ms = new System.IO.MemoryStream(email.MimeContent.Content))
			//{
			//	var mimeMsg = MimeKit.MimeMessage.Load(ms);
			//	return Imap.ImapMessage.FromMessage(mimeMsg, id);
			//}

			throw new NotImplementedException();
		}

		public void MarkMessagesAsRead(string[] ids)
		{
			throw new NotImplementedException();
		}

		public void MoveMessages(string[] ids)
		{
			throw new NotImplementedException();
		}

		public IList<string> GetFolderNames(FolderType type)
		{
			throw new NotImplementedException();
		}

		public IMessage LoadMessage(string fileName, string id)
		{
			throw new NotImplementedException();
		}

		public void SaveMessage(string fileName, string id)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			_folderToMoveID = null;
			_folderID = null;
			_service = null;
			ServicePointManager.ServerCertificateValidationCallback = _oldGlobalCertificateValidationCallback;
		}

		#endregion

		private void EnsureInitialized(bool withFolderToMove)
		{
			if (_service == null)
			{
				var serverVersion = _serverVersion; // TODO: switch on GSS type.
				var protocol = _config.UseSsl ? "https" : "http";

				_oldGlobalCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback;
				ServicePointManager.ServerCertificateValidationCallback = NetworkHelper.ServerCertificateValidationCallback;

				_service = new ExchangeService(serverVersion)
								{
									Url = new Uri(String.Format(_serviceUrlFormat, protocol, _config.Host)),
									Credentials = new NetworkCredential(_config.Username, _config.Password),
									PreAuthenticate = true,
									Timeout = 10 * 1000
								};
			}

			if (_folderID == null)
			{
				_folderID = FindFolderID(_config.Folder.Name, null, _config.Folder.Type);
			}

			if (withFolderToMove && _folderToMoveID == null && _config.FolderToMove != null)
			{
				_folderID = FindFolderID(_config.FolderToMove.Name, null, _config.FolderToMove.Type);
			}
		}

		private IList<string> GetMails(bool unreadOnly)
		{
			var filter = new SearchFilter.SearchFilterCollection(
															LogicalOperator.And,
															new SearchFilter.IsNotEqualTo(ItemSchema.ItemClass, ExchangeConstants.UndeliverableMessageClass)
														);
			if (unreadOnly)
			{
				filter.Add(new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));
			}

			if (_config.IgnoreAbsenceEmails)
			{
				filter.Add(new SearchFilter.IsNotEqualTo(ItemSchema.ItemClass, ExchangeConstants.OutOfOfficeMessageClass));
			}

			return SearchMailIDs(filter);
		}

		private IList<string> SearchMailIDs(SearchFilter filter)
		{
			EnsureInitialized(false);

			var folder = Microsoft.Exchange.WebServices.Data.Folder.Bind(_service, _folderID);
			var resultIDs = new List<string>();
			var offset = 0;
			FindItemsResults<Item> result;

			do
			{
				var view = new ItemView(_messageMaxCount, offset) {PropertySet = PropertySet.IdOnly};
				result = folder.FindItems(filter, view);
				offset += result.Items.Count;
				resultIDs.AddRange(result.Items.Select(it => it.Id.UniqueId));
			}
			while (result.MoreAvailable && offset < result.TotalCount);

			return resultIDs.AsReadOnly();
		}

		private FolderId FindFolderID(string folderName, FolderId parentFolderId, FolderType folderType)
		{
			var folder = GetRootFolder(folderType, parentFolderId);

			return FindChildFolderByDisplayName(folder, folderName)?.Id;
		}

		private Folder GetRootFolder(FolderType folderType, FolderId folderId)
		{
			if (folderId != null)
			{
				return Microsoft.Exchange.WebServices.Data.Folder.Bind(_service, folderId);
			}

			// EWS does not support retrieving Public Folder by FolderID of Public Folders root.
			// Retrieve Public Folders root by well-known name.
			return folderType == FolderType.Public
						? Microsoft.Exchange.WebServices.Data.Folder.Bind(_service, WellKnownFolderName.PublicFoldersRoot)
						: Microsoft.Exchange.WebServices.Data.Folder.Bind(_service, GetRootFolderID(folderType));
		}

		private FolderId GetRootFolderID(FolderType folderType)
		{
			var mailAddress = NetworkHelper.GetMailAddress(_config);
			return mailAddress != null
						? new FolderId(GetFolderType(folderType), mailAddress)
						: new FolderId(GetFolderType(folderType));
		}

		private Item GetItem(string itemID)
		{
			var props = new PropertySet(BasePropertySet.FirstClassProperties, new PropertySet(ItemSchema.MimeContent));
			return Item.Bind(_service, new ItemId(itemID), props);
		}
		/*
		private IEnumerable<FolderInfo> GetFoldersRecursive(FolderInfo parentFolder, Predicate<MxFolder> filter, bool skipParent)
		{
			if (skipParent || filter == null || filter(parentFolder.Folder))
			{
				string parentName = null;

				if (!skipParent)
				{
					parentName = parentFolder.FullName;
					yield return parentFolder;
				}

				var findFoldersResult = parentFolder.Folder.FindFolders(new FolderView(_subfolderLimit));
				var children = findFoldersResult?.Folders;

				if (children != null)
				{
					foreach (var child in children)
					{
						var childName = parentName != null
							? String.Join(FolderSeparator, parentName, child.DisplayName)
							: child.DisplayName;
						var childInfo = new FolderInfo(childName, child);
						foreach (var folder in GetFoldersRecursive(childInfo, filter, false))
						{
							yield return folder;
						}
					}
				}
			}
		}
		*/

		private static WellKnownFolderName GetFolderType(FolderType type)
		{
			switch (type)
			{
				case FolderType.Message:
					return WellKnownFolderName.MsgFolderRoot;

				case FolderType.Calendar:
					return WellKnownFolderName.Calendar;

				case FolderType.Task:
					return WellKnownFolderName.Tasks;

				case FolderType.Public:
					return WellKnownFolderName.PublicFoldersRoot;

				default:
					return WellKnownFolderName.MsgFolderRoot;
			}
		}

		private static Folder FindChildFolderByDisplayName(Folder folder, string name)
		{
			int separatorIndex;

			do
			{
				var currentName = name;

				separatorIndex = name.IndexOf(_folderSeparator, StringComparison.Ordinal);

				//If folder path containg more the one folder, search folder one by one
				if (separatorIndex >= 0)
				{
					currentName = name.Substring(0, separatorIndex);
					name = name.Substring(separatorIndex + 1);
				}

				folder = folder.FindFolders(new FolderView(_subfolderLimit))
								.FirstOrDefault(f => f.DisplayName.Equals(currentName, StringComparison.InvariantCultureIgnoreCase));

				if (folder == null)
				{
					break;
				}
			} while (separatorIndex >= 0);

			return folder;
		}
	}
}
