
namespace Matrix42.Client.Mail
{
    public interface IAttachment
    {
		string Name { get; }

		string Cid { get; }

		string MimeType { get; }

		//string ContentLocation { get; }

		byte[] Data { get; }
    }
}
