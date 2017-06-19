using System.IO;

namespace Matrix42.Client.Mail.Utility
{
	internal interface IWriteable
	{
		void WriteTo(Stream stream);
	}
}
