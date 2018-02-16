using System;
using System.Runtime.Serialization;

namespace Matrix42.Client.Mail.Contracts
{
	[Serializable]
	[DataContract]
    public class MailAddress
    {
		[DataMember]
		public string Address { get; set; }

		[DataMember]
		public string DisplayName { get; set; }

	    public override string ToString()
	    {
		    return (!String.IsNullOrEmpty(DisplayName) ? $"\"{DisplayName}\" " : String.Empty) + Address;
	    }
    }
}
