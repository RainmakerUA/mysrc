using System;

namespace Matrix42.Client.Mail.Contracts
{
	[Serializable]
    public enum FolderType
    {
		Unknown = 0,
	    Message = 1,
	    Public = 2,
	    Task = 3,
	    Calendar = 4
	}
}
