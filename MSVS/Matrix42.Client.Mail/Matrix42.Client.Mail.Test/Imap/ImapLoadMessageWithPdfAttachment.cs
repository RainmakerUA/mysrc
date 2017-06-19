using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Matrix42.Client.Mail.Test.Imap
{
	[TestClass]
	[DeploymentItem("Files")]
	public class ImapLoadMessageWithPdfAttachment : ImapLoadMessageTestBase
	{
		/// <summary>
		/// PRB28094: PDF attachments of E-Mails with Content-Transfer-Encoding: quoted-printable are broken by E-Mail Robot
		/// pdf attachments are imported not correct and could be opened after import
		/// </summary>
		[TestMethod]
		public void CheckMailWithPdfAttachmentTest()
		{
			var message = GetMessage("FileWithPDFAttachments.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(1, message.Attachments.Count);

			var attachment = message.Attachments[0];

			Assert.IsNotNull(attachment);
			Assert.AreEqual(40337, attachment.Data.Length);

			//Was before (with trailing newline?): 53f1facc28f0c4d728233653073df30f
			Assert.AreEqual("34f70ec1a8da16b9c81d0f472efc1870", GetFileHash(attachment.Data));
		}

		private static string GetFileHash(byte[] bytes)
		{
			string result;

			using (var md5 = MD5.Create())
			{
				result = BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", "").ToLower();
			}

			return result;
		}
	}
}
