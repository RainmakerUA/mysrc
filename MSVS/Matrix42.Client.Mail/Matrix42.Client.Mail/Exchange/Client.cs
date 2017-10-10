using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using Matrix42.Client.Mail.Contracts;
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

		public IReadOnlyList<string> GetUnreadMails()
		{
			return GetMails(true);
		}

		public IReadOnlyList<string> GetAllMails()
		{
			return GetMails(false);
		}

		public IReadOnlyList<string> SearchMails(string[] terms)
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

			var item = GetItem(id, ItemSchema.TextBody, ItemSchema.Attachments);
			
			return Message.FromItem(item, id);

			// Optional impl.: use MimeKit to parse mail
			// var item = GetItem(id, new PropertySet(BasePropertySet.IdOnly, ItemSchema.MimeContent));

			//using (var ms = new System.IO.MemoryStream(item.MimeContent.Content))
			//{
			//	var mimeMsg = MimeKit.MimeMessage.Load(ms);
			//	return Imap.ImapMessage.FromMessage(mimeMsg, id);
			//}
		}

		public void MarkMessagesAsRead(string[] ids)
		{
			if (ids != null)
			{
				EnsureInitialized(false);

				foreach (var id in ids)
				{
					if (GetItem(id) is EmailMessage message)
					{
						message.IsRead = true;
						message.Update(ConflictResolutionMode.AutoResolve);
					}
				}
			}
		}

		public void MoveMessages(string[] ids)
		{
			if (ids != null)
			{
				EnsureInitialized(true);

				foreach (var id in ids)
				{
					GetItem(id)?.Move(_folderToMoveID);
				}
			}
		}

		public IReadOnlyList<string> GetFolderNames(FolderType type)
		{
			EnsureInitialized(false);

			var rootFolderID = GetRootFolderID(type);
			var result = _service.FindFolders(rootFolderID, new FolderView(Int32.MaxValue) { Traversal = FolderTraversal.Deep });
			var idFolders = new Dictionary<string, string>();

			foreach (var folder in result.Folders.Where(IsMessageFolder))
			{
				var uniqueId = folder.Id.UniqueId;
				var fullName = (idFolders.TryGetValue(folder.ParentFolderId.UniqueId, out var parentName) ? parentName + _folderSeparator : String.Empty) + folder.DisplayName;

				if (!idFolders.ContainsKey(uniqueId))
				{
					idFolders.Add(uniqueId, fullName);
				}
			}

			return idFolders.Values.OrderBy(s => s).ToArray();
		}

		public IMessage LoadMessage(string fileName, string id)
		{
			throw new NotImplementedException();
		}

		public void SaveMessage(string fileName, string id)
		{
			// GetItem(id, new PropertySet(BasePropertySet.IdOnly, ItemSchema.MimeContent)) is EmailMessage message

			/*
			var messageContent = "N/A";

			try
			{
				var item = GetItem(id);

				if (item?.MimeContent?.Content != null && item.MimeContent.Content.Length > 0)
				{
					var encoding = Encoding.GetEncoding(item.MimeContent.CharacterSet);
					messageContent = encoding.GetString(item.MimeContent.Content);
				}
			}
			catch (Exception)
			{
				// Ignore exceptions
			}

			FileHelper.WriteToFile(filePath, messageContent);
			*/

			var content = GetItem(id, new PropertySet(BasePropertySet.IdOnly, ItemSchema.MimeContent))?.MimeContent?.Content;

			if (content != null)
			{
				FileHelper.SaveMimeContent(content, fileName, DecodeID(id));
			}
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
				_folderToMoveID = FindFolderID(_config.FolderToMove.Name, null, _config.FolderToMove.Type);
			}
		}

		private IReadOnlyList<string> GetMails(bool unreadOnly)
		{
			var filter = new SearchFilter.SearchFilterCollection(
															LogicalOperator.And,
															new SearchFilter.IsNotEqualTo(ItemSchema.ItemClass, Constants.UndeliverableMessageClass)
														);
			if (unreadOnly)
			{
				filter.Add(new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));
			}

			if (_config.IgnoreAbsenceEmails)
			{
				filter.Add(new SearchFilter.IsNotEqualTo(ItemSchema.ItemClass, Constants.OutOfOfficeMessageClass));
			}

			return SearchMailIDs(filter);
		}

		private IReadOnlyList<string> SearchMailIDs(SearchFilter filter)
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

		private Item GetItem(string itemID, params PropertyDefinitionBase[] additionalProperties)
		{
			var props = new PropertySet(BasePropertySet.FirstClassProperties, additionalProperties);
			return GetItem(itemID, props);
		}

		private Item GetItem(string itemID, PropertySet properties)
		{
			return Item.Bind(_service, new ItemId(itemID), properties);
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

		private static bool IsMessageFolder(Folder f)
		{
			return f.FolderClass != null && (f.FolderClass.Equals("IPF.Note") || f.FolderClass.Equals("IPF.StickyNote"));
		}

		private static string DecodeID(string exchangeID)
		{
			return Encoding.ASCII.GetString(Convert.FromBase64String(exchangeID)).Substring(4, 36);
		}
	}
}
