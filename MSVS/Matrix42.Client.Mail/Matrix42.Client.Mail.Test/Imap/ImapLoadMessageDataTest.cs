using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Matrix42.Client.Mail.Imap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Matrix42.Client.Mail.Test.Imap
{
	/// <summary>
	/// This test class tests <see cref="T:Matrix42.Client.Mail.Imap.ImapClient"/>
	/// It checks data parsed by client.
	/// </summary>
	[TestClass]
	[DeploymentItem("Files")]
	public class ImapLoadMessageDataTest : ImapLoadMessageTestBase
	{
		// TODO: Refactor with LoadAndAssertMessage(...)

		[TestMethod]
		public void SubjectSpecialSymbols01Test()
		{
			var message = GetMessage("SubjectSpecialSymbols01.emr");

			Assert.AreEqual("!@#$%^&*() ÄÖöäÜÜÜ_", message.Subject);
		}

		/// <summary>
		/// Subject contains partially encoded text
		/// </summary>
		[TestMethod]
		public void SubjectSpecialSymbols02Test()
		{
			var message = GetMessage("SubjectSpecialSymbols02.emr");

			Assert.AreEqual("Verlängerung Dienstbescheinigung", message.Subject);
		}

		/// <summary>
		/// Subject is multiline text with space separator in the beginning of lines
		/// </summary>
		[TestMethod]
		public void MultilineSubjectWithSpaceSeparatorTest()
		{
			var message = GetMessage("MultilineSubjectWithSpaceSeparator.emr");

			Assert.AreEqual("abc", message.Subject);
		}

		/// <summary>
		/// Subject is non-encoded multiline text
		/// </summary>
		[TestMethod]
		public void MultilineNonEncodedSubjectTest()
		{
			var message = GetMessage("MultilineNonEncodedSubject.emr");

			Assert.AreEqual("a b c", message.Subject);
		}

		[TestMethod]
		public void TextAttachment001Test()
		{
			const string attachmentString = "AttachmentFileContent";

			var message = GetMessage("TextAttachment001.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(1, message.Attachments.Count);
			Assert.IsInstanceOfType(message.Attachments[0], typeof(ImapAttachment));
			Assert.AreEqual("a.txt", message.Attachments[0].Name);
			
			var expectedAttachmentData = new byte[44];
			expectedAttachmentData[0] = 255;
			expectedAttachmentData[1] = 254;
			
			for (var i = 0; i < attachmentString.Length; i++)
			{
				expectedAttachmentData[2 * i + 2] = (byte)attachmentString[i];
				expectedAttachmentData[2 * i + 3] = 0;
			}

			CollectionAssert.AreEqual(expectedAttachmentData, message.Attachments[0].Data);
		}

		/// <summary>
		/// Mail created via Outlook by attachning outlook item (existing mail message)
		/// </summary>
		[TestMethod]
		public void TestEmlAttachments()
		{
			var message = GetMessage("EmlAttachments.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(2, message.Attachments.Count);
			Assert.AreEqual("File.eml", message.Attachments[0].Name);
			Assert.AreEqual("File.eml", message.Attachments[1].Name);
		}

		/// <summary>
		/// Mail created via Outlook by attachning outlook item (existing mail message)
		/// </summary>
		[TestMethod]
		public void TestEmlAttachments02()
		{
			var message = GetMessage("EmlAttachments02.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(1, message.Attachments.Count);
			Assert.AreNotEqual("File.eml", message.Attachments[0].Name);
		}

		/// <summary>
		/// Mail created via Outlook by attachning outlook item (existing mail message)
		/// </summary>
		[TestMethod]
		public void TestEmlAttachments03()
		{
			var message = GetMessage("EmlAttachments03.emr");

			Assert.AreEqual(1, message.Attachments.Count);
			Assert.AreNotEqual("File.eml", message.Attachments[0].Name);
		}

		[TestMethod]
		public void TextAttachment002Test()
		{
			var message = GetMessage("TextAttachment002.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(2, message.Attachments.Count);
			Assert.AreEqual("5x5.pdf", message.Attachments[0].Name);
			Assert.AreEqual("smime.p7s", message.Attachments[1].Name);
		}

		[TestMethod]
		public void TextAttachment003Test()
		{
			var message = GetMessage("TextAttachment003.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(1, message.Attachments.Count);
			Assert.AreEqual("4281e4ad931.0e50_0.pdf", message.Attachments[0].Name);
		}

		/// <summary>
		/// File name mentioned only in Content-Disposition and name not surrounded with qoutes
		/// </summary>
		[TestMethod]
		public void TextAttachment004Test()
		{
			var message = GetMessage("TextAttachment004.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(1, message.Attachments.Count);
			Assert.AreEqual("a.txt", message.Attachments[0].Name);
		}

		/// <summary>
		/// File name very long and specified in two strings
		/// </summary>
		[TestMethod]
		public void TextAttachment005Test()
		{
			var message = GetMessage("TextAttachment005.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(4, message.Attachments.Count);
			Assert.AreEqual("1.txt", message.Attachments[0].Name);
			Assert.AreEqual("2.txt", message.Attachments[1].Name);
			Assert.AreEqual("Bedienungsanleitung _Videokonferenzanlage_ HG-Informatikraum.txt", message.Attachments[2].Name);
			Assert.AreEqual("4.txt", message.Attachments[3].Name);
		}

		/// <summary>
		/// File name contains symbols in ISO-8859-1 encoding.
		/// TextAttachment006.dat file saved in Windows-1252 encoding.
		/// </summary>
		[TestMethod]
		public void TextAttachment006Test()
		{
			var message = GetMessage("TextAttachment006.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(4, message.Attachments.Count);
			Assert.AreEqual("1üöä.txt", message.Attachments[0].Name);
			Assert.AreEqual("2.txt", message.Attachments[1].Name);
			Assert.AreEqual("3.txt", message.Attachments[2].Name);
			Assert.AreEqual("4.txt", message.Attachments[3].Name);
		}

		/// <summary>
		/// File name was incorrectly encoded.
		/// File name should be set to default and extension should be found in registry by Content Type
		/// </summary>
		[TestMethod]
		public void InvalidEncodedExtensionTest()
		{
			//IMAP.IMAP.ExtensionResolver =
			//		new FakeExtensionResolver(new Dictionary<string, string> { { "application/vnd.oasis.opendocument.text", ".odt" } });
			var message = GetMessage("InvalidEncodedExtension.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(1, message.Attachments.Count);
			Assert.AreEqual("Löschung Nutzer+_.odt", message.Attachments[0].Name);
		}

		/// <summary>
		/// From field is encoded
		/// </summary>
		[TestMethod]
		public void FromFieldIsEncodedTest()
		{
			var message = GetMessage("FromFieldIsEncoded.emr");

			Assert.AreEqual("Сергей Мороз", message.From.DisplayName);
		}

		/// <summary>
		/// File name specified twice in one line
		/// </summary>
		[TestMethod]
		public void AttachmentWithDuplicationDataInHeaderTest()
		{
			var message = GetMessage("AttachmentWithDuplicationDataInHeader.emr");

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(1, message.Attachments.Count);
			Assert.AreEqual("a.txt", message.Attachments[0].Name);
		}

		/// <summary>
		/// Boundary wrapped into next line
		/// </summary>
		[TestMethod]
		public void BoundaryWrappedTest()
		{
			var message = GetMessage("BoundaryWrapped.emr");

			Assert.IsFalse(string.IsNullOrEmpty(message.BodyText));
			Assert.IsFalse(string.IsNullOrEmpty(message.BodyHtml));
		}

		/// <summary>
		/// Message contains embedded attachment
		/// </summary>
		[TestMethod]
		public void EmbeddedAttachment001Test()
		{
			var message = GetMessage("EmbeddedAttachment001.emr");

			const string attachmentCid = "783ED9766EB13D3FFEF0BA8DCFE4A0D2B126A9A5@eurprd04.prod.outlook.com";

			StringAssert.Contains(message.BodyHtml, "html");
			StringAssert.Contains(message.BodyHtml, attachmentCid);
			Assert.IsFalse(string.IsNullOrEmpty(message.BodyText));

			Assert.IsNotNull(message.Attachments);
			Assert.AreEqual(1, message.Attachments.Count);
			Assert.AreEqual("Picture (Device Independent Bitmap) 1.jpg", message.Attachments[0].Name);
			Assert.AreEqual("783ED9766EB13D3FFEF0BA8DCFE4A0D2B126A9A5@eurprd04.prod.outlook.com", message.Attachments[0].Cid);
		}

		/// <summary>
		/// Message contains body that must not be encoded
		/// </summary>
		[TestMethod]
		public void NonEncodedBodyTest()
		{
			var message = GetMessage("NonEncodedBody.emr");
			Assert.AreEqual("OMNI_P 130314110647 Inst=55 BST=6010 Geraet=61090 obere Tuer offen          \r\n", message.BodyText);
		}

		/// <summary>
		/// Message contains UTF8 encoded body
		/// </summary>
		[TestMethod]
		public void UTF8EncodedBodyTest()
		{
			var message = GetMessage("UTF8EncodedBody.emr");
			Assert.AreEqual("Liebe Kollegen,A", message.BodyText);
		}

		/// <summary>
		/// Message contains UTF8 encoded body
		/// </summary>
		[TestMethod]
		public void TestUTF8EncodedBody2()
		{
			var message = GetMessage("UTF8EncodedBody2.emr");
			Assert.IsTrue(message.BodyHtml.Contains("SMO_Deutsch_!!!!! &nbsp;- ß!&quot;§$%&amp;/(/)=?`*'Ä*Ü_ÄÜ*Ü*ÄÖÖÄÄÜ?"));
		}

		/// <summary>
		/// Message contains 3-byte encoded UTF-8 characters.
		/// </summary>
		[TestMethod]
		public void Test3byteUtfCharacters()
		{
			const char fail = '\uFFFD';

			var message = GetMessage("UTF8EncodedBody3.emr");
			Assert.IsTrue(!String.IsNullOrEmpty(message.BodyText) && message.BodyText.IndexOf(fail) == -1);
		}

		/// <summary>
		/// Message is in multipart/mixed encoding (2+ parts with individual encoding specified).
		/// Message has plaintext part with 'quoted-printable' and attachment with 'base64'
		/// </summary>
		[TestMethod]
		public void TestMultipartMixed()
		{
			var message = GetMessage("MultipartMixed.emr");

			Assert.IsFalse(String.IsNullOrEmpty(message.BodyText));
			Assert.IsTrue(String.IsNullOrEmpty(message.BodyHtml));
		}

		/// <summary>
		/// Plain-text message (no boundaries) with fetch result line containing parentheses.
		/// (E.g. server response end with "IMAP006 OK Completed (0.000 sec)")
		/// </summary>
		[TestMethod]
		public void TestFetchResultWithParentheses()
		{
			const string fetchResult = "IMAP006 OK Completed";

			var message = GetMessage("FetchResultWithParentheses.emr");

			Assert.IsFalse(message.BodyText.Contains(fetchResult));
		}

		/// <summary>
		/// Plain-text message (no boundaries) with fetch result line containing parentheses.
		/// Message body contains a string ending with valid body terminator (" UID xxx)")
		/// (E.g. server response end with "IMAP006 OK Completed (0.000 sec)")
		/// </summary>
		[TestMethod]
		public void TestFetchResultWithParenthesesLineWithUid()
		{
			const string fetchResult = "IMAP006 OK Completed";

			var message = GetMessage("FetchResultWithParenthesesLineWithUid.emr");

			Assert.IsFalse(message.BodyText.Contains(fetchResult));

			var bodyLines = message.BodyText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var lineCounts = new Dictionary<string, int>();

			foreach (var line in bodyLines)
			{
				if (lineCounts.ContainsKey(line))
				{
					lineCounts[line] += 1;
				}
				else
				{
					lineCounts[line] = 1;
				}
			}

			Assert.IsTrue(lineCounts.Values.All(count => count == 1));
		}

		[TestMethod]
		public void TestMessageAttachmentsParsing()
		{
			var msg = GetMessage("0_HierDieMails.emr");

			Assert.IsFalse(String.IsNullOrEmpty(msg.BodyText));
			Assert.IsFalse(String.IsNullOrEmpty(msg.BodyHtml));
			Assert.IsNotNull(msg.Attachments);
			Assert.AreEqual(2, msg.Attachments.Count);

			Assert.IsTrue(msg.Attachments.All(att => att.MimeType == "message/rfc822"));
			Assert.IsFalse(msg.Attachments.Any(att => att.Name == "File.eml"));
		}

		[TestMethod]
		public void TestAttchmentQuotedPrintableWithoutCharset()
		{
			var msg = GetMessage("1_InitScripts.emr");

			Assert.IsNotNull(msg.Attachments);
			Assert.AreEqual(3, msg.Attachments.Count);

			Assert.IsTrue(msg.Attachments.All(att => att.Name.EndsWith(".txt")));
			Assert.IsTrue(msg.Attachments.All(att => !Encoding.UTF8.GetString(att.Data).Contains(InvalidCharacter)));
		}

		[TestMethod]
		public void TestContentDispositionInlineAttchment()
		{
			var msg = GetMessage("2_Re_Call.emr");

			Assert.IsNotNull(msg.Attachments);
			Assert.AreEqual(1, msg.Attachments.Count);
			Assert.AreEqual("1204_ESIRU_cTTour_20160610084453013_84638.xml", msg.Attachments[0].Name);
		}

		[TestMethod]
		public void Test7BitCsvAttchmentQuotes()
		{
			var msg = GetMessage("3_Buchungen.emr");

			Assert.IsNotNull(msg.Attachments);
			Assert.AreEqual(1, msg.Attachments.Count);
			Assert.AreEqual("SoftwareServiceBuchungen_TargetCC_unequal_BuchungCC.csv", msg.Attachments[0].Name);
		}

		[TestMethod]
		public void TestAttchmentNameParsingNoQuotes()
		{
			var msg = GetMessage("4_#R00003575.emr");

			Assert.IsNotNull(msg.Attachments);
			Assert.AreEqual(1, msg.Attachments.Count);
			Assert.AreEqual("160912_Korrekturen.xlsx", msg.Attachments[0].Name);
		}

		[TestMethod]
		public void TestSectionCharsetCpX()
		{
			var msg = GetMessage("5_DaimlerTest.emr");

			Assert.IsFalse(String.IsNullOrEmpty(msg.BodyText));
			Assert.IsFalse(msg.BodyText.Contains("Failed to decode"));

			Assert.IsNotNull(msg.Attachments);
			Assert.AreEqual(1, msg.Attachments.Count);
			Assert.AreEqual("11851D6Z2.emr", msg.Attachments[0].Name);
		}

		[TestMethod]
		public void TestMultipartRelatedWithBoundary()
		{
			var msg = GetMessage("MultipartRelatedWithBoundary.emr");

			Assert.IsFalse(String.IsNullOrEmpty(msg.BodyText));
			Assert.IsTrue(msg.BodyText.StartsWith("Guten Tag,", StringComparison.Ordinal));

			Assert.IsFalse(String.IsNullOrEmpty(msg.BodyHtml));
			Assert.IsTrue(msg.BodyHtml.StartsWith("<font size=2", StringComparison.Ordinal));

			Assert.IsNotNull(msg.Attachments);
			Assert.AreEqual(1, msg.Attachments.Count);
			Assert.AreEqual("image/jpeg", msg.Attachments[0].MimeType);
		}

		[TestMethod]
		public void TestReadMailEncodingWith8bit()
		{
			var msg = GetMessage("8bitSection.emr");

			Assert.IsFalse(String.IsNullOrEmpty(msg.BodyText));
			Assert.IsFalse(msg.BodyText.Contains(InvalidCharacter));
		}
	}
}
