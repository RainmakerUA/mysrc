using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		[TestMethod]
		public void SubjectSpecialSymbols01Test()
		{
			AssertMessageSubjectEqual("SubjectSpecialSymbols01.emr", "!@#$%^&*() ÄÖöäÜÜÜ_");
		}

		/// <summary>
		/// Subject contains partially encoded text
		/// </summary>
		[TestMethod]
		public void SubjectSpecialSymbols02Test()
		{
			AssertMessageSubjectEqual("SubjectSpecialSymbols02.emr", "Verlängerung Dienstbescheinigung");
		}

		/// <summary>
		/// Subject is multiline text with space separator in the beginning of lines
		/// </summary>
		[TestMethod]
		public void MultilineSubjectWithSpaceSeparatorTest()
		{
			AssertMessageSubjectEqual("MultilineSubjectWithSpaceSeparator.emr", "abc");
		}

		/// <summary>
		/// Subject is non-encoded multiline text
		/// </summary>
		[TestMethod]
		public void MultilineNonEncodedSubjectTest()
		{
			AssertMessageSubjectEqual("MultilineNonEncodedSubject.emr", "a b c");
		}

		[TestMethod]
		public void TextAttachment001Test()
		{
			const string attachmentString = "AttachmentFileContent";

			var expectedAttachmentData = new byte[44];
			expectedAttachmentData[0] = 255;
			expectedAttachmentData[1] = 254;

			for (var i = 0; i < attachmentString.Length; i++)
			{
				expectedAttachmentData[2 * i + 2] = (byte)attachmentString[i];
				expectedAttachmentData[2 * i + 3] = 0;
			}

			LoadAndAssertMessage(
							"TextAttachment001.emr",
							new MessageValidateParameters { Attachments = 1 },
							msg =>
								{
									Assert.AreEqual("a.txt", msg.Attachments[0].Name);
									CollectionAssert.AreEqual(expectedAttachmentData, msg.Attachments[0].Data);
								}
						);
		}

		/// <summary>
		/// Mail created via Outlook by attachning outlook item (existing mail message)
		/// </summary>
		[TestMethod]
		public void TestEmlAttachments()
		{
			AssertAttachmentNames("EmlAttachments.emr", "File.eml", "File.eml");
		}

		/// <summary>
		/// Mail created via Outlook by attachning outlook item (existing mail message)
		/// </summary>
		[TestMethod]
		public void TestEmlAttachments02()
		{
			AssertAttachmentNames("EmlAttachments02.emr", "Test_ no attachments.eml");
		}

		/// <summary>
		/// Mail created via Outlook by attachning outlook item (existing mail message)
		/// </summary>
		[TestMethod]
		public void TestEmlAttachments03()
		{
			AssertAttachmentNames("EmlAttachments03.emr", "Testmail von Outlook über Exchange Server benötigt.eml");
		}

		[TestMethod]
		public void TextAttachment002Test()
		{
			AssertAttachmentNames("TextAttachment002.emr", "5x5.pdf", "smime.p7s");
		}

		[TestMethod]
		public void TextAttachment003Test()
		{
			AssertAttachmentNames("TextAttachment003.emr", "4281e4ad931.0e50_0.pdf");
		}

		/// <summary>
		/// File name mentioned only in Content-Disposition and name not surrounded with qoutes
		/// </summary>
		[TestMethod]
		public void TextAttachment004Test()
		{
			AssertAttachmentNames("TextAttachment004.emr", "a.txt");
		}

		/// <summary>
		/// File name very long and specified in two strings
		/// </summary>
		[TestMethod]
		public void TextAttachment005Test()
		{
			AssertAttachmentNames(
							"TextAttachment005.emr",
							"1.txt", "2.txt", "Bedienungsanleitung _Videokonferenzanlage_ HG-Informatikraum.txt", "4.txt"
						);
		}

		/// <summary>
		/// File name contains symbols in ISO-8859-1 encoding.
		/// TextAttachment006.dat file saved in Windows-1252 encoding.
		/// </summary>
		[TestMethod]
		public void TextAttachment006Test()
		{
			AssertAttachmentNames(
							"TextAttachment006.emr",
							"1üöä.txt", "2.txt", "3.txt", "4.txt"
						);
		}

		/// <summary>
		/// File name was incorrectly encoded.
		/// File name should be set to default and extension should be found in registry by Content Type
		/// </summary>
		[TestMethod]
		public void InvalidEncodedExtensionTest()
		{
			AssertAttachmentNames("InvalidEncodedExtension.emr", "Löschung Nutzer+_.odt");
		}

		/// <summary>
		/// From field is encoded
		/// </summary>
		[TestMethod]
		public void FromFieldIsEncodedTest()
		{
			LoadAndAssertMessage(
							"FromFieldIsEncoded.emr",
							new MessageValidateParameters { From = true },
							msg => Assert.AreEqual("Сергей Мороз", msg.From.DisplayName)
						);
		}

		/// <summary>
		/// File name specified twice in one line
		/// </summary>
		[TestMethod]
		public void AttachmentWithDuplicationDataInHeaderTest()
		{
			AssertAttachmentNames("AttachmentWithDuplicationDataInHeader.emr", "a.txt");
		}

		/// <summary>
		/// Boundary wrapped into next line
		/// </summary>
		[TestMethod]
		public void BoundaryWrappedTest()
		{
			LoadAndAssertMessage(
							"BoundaryWrapped.emr",
							new MessageValidateParameters { BodyText = true, BodyHtml = true }
						);
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
			LoadAndAssertMessage(
							"NonEncodedBody.emr",
							new MessageValidateParameters { BodyText = true },
							msg => Assert.IsTrue(msg.BodyText.Equals("OMNI_P 130314110647 Inst=55 BST=6010 Geraet=61090 obere Tuer offen          \r\n", StringComparison.Ordinal))
					);
		}

		/// <summary>
		/// Message contains UTF8 encoded body
		/// </summary>
		[TestMethod]
		public void UTF8EncodedBodyTest()
		{
			LoadAndAssertMessage(
							"UTF8EncodedBody.emr",
							new MessageValidateParameters { BodyText = true },
							msg => Assert.IsTrue(msg.BodyText.Equals("Liebe Kollegen,A", StringComparison.Ordinal))
					);
		}

		/// <summary>
		/// Message contains UTF8 encoded body
		/// </summary>
		[TestMethod]
		public void TestUTF8EncodedBody2()
		{
			LoadAndAssertMessage(
							"UTF8EncodedBody2.emr",
							new MessageValidateParameters { BodyHtml = true },
							msg => Assert.IsTrue(msg.BodyHtml.IndexOf("SMO_Deutsch_!!!!! &nbsp;- ß!&quot;§$%&amp;/(/)=?`*'Ä*Ü_ÄÜ*Ü*ÄÖÖÄÄÜ?", StringComparison.Ordinal) >= 0)
						);
		}

		/// <summary>
		/// Message contains 3-byte encoded UTF-8 characters.
		/// </summary>
		[TestMethod]
		public void Test3byteUtfCharacters()
		{
			LoadAndAssertMessage(
							"UTF8EncodedBody3.emr",
							new MessageValidateParameters { BodyText = true },
							msg => Assert.IsTrue(msg.BodyText.IndexOf(InvalidCharacter) == -1)
						);
		}

		/// <summary>
		/// Message is in multipart/mixed encoding (2+ parts with individual encoding specified).
		/// Message has plaintext part with 'quoted-printable' and attachment with 'base64'
		/// </summary>
		[TestMethod]
		public void TestMultipartMixed()
		{
			LoadAndAssertMessage(
							"MultipartMixed.emr",
							new MessageValidateParameters { BodyText = true, BodyHtml = false }
						);
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
			LoadAndAssertMessage(
							"0_HierDieMails.emr",
							new MessageValidateParameters { BodyText = true, BodyHtml = true, Attachments = 2 },
							msg =>
								{
									Assert.IsTrue(msg.Attachments.All(att => att.MimeType == "message/rfc822"));
									Assert.IsFalse(msg.Attachments.Any(att => att.Name == "File.eml"));
								}
						);
		}

		[TestMethod]
		public void TestAttchmentQuotedPrintableWithoutCharset()
		{
			LoadAndAssertMessage(
							"1_InitScripts.emr",
							MessageValidateParameters.GetAttachments(3),
							msg =>
								{
									Assert.IsTrue(msg.Attachments.All(att => att.Name.EndsWith(".txt")));
									Assert.IsTrue(msg.Attachments.All(att => Encoding.UTF8.GetString(att.Data).IndexOf(InvalidCharacter) == -1));
								}
						);
		}

		[TestMethod]
		public void TestContentDispositionInlineAttchment()
		{
			AssertAttachmentNames("2_Re_Call.emr", "1204_ESIRU_cTTour_20160610084453013_84638.xml");
		}

		[TestMethod]
		public void Test7BitCsvAttchmentQuotes()
		{
			AssertAttachmentNames("3_Buchungen.emr", "SoftwareServiceBuchungen_TargetCC_unequal_BuchungCC.csv");
		}

		[TestMethod]
		public void TestAttchmentNameParsingNoQuotes()
		{
			AssertAttachmentNames("4_#R00003575.emr", "160912_Korrekturen.xlsx");
		}

		[TestMethod]
		public void TestSectionCharsetCpX()
		{
			LoadAndAssertMessage(
							"5_DaimlerTest.emr",
							new MessageValidateParameters { BodyText = true, Attachments = 1 },
							msg =>
								{
									Assert.IsFalse(msg.BodyText.Contains("Failed to decode"));
									Assert.AreEqual("11851D6Z2.DAT", msg.Attachments[0].Name);
								}
						);
			
		}

		[TestMethod]
		public void TestMultipartRelatedWithBoundary()
		{
			LoadAndAssertMessage(
							"MultipartRelatedWithBoundary.emr",
							new MessageValidateParameters { BodyText = true, BodyHtml = true, Attachments = 1 },
							msg =>
								{
									Assert.IsTrue(msg.BodyText.StartsWith("Guten Tag,", StringComparison.Ordinal));
									Assert.IsTrue(msg.BodyHtml.StartsWith("<font size=2", StringComparison.Ordinal));
									Assert.AreEqual("image/jpeg", msg.Attachments[0].MimeType);
								}
						);
		}

		[TestMethod]
		public void TestReadMailEncodingWith8bit()
		{
			LoadAndAssertMessage(
							"8bitSection.emr",
							new MessageValidateParameters { BodyText = true },
							msg => Assert.IsTrue(msg.BodyText.IndexOf(InvalidCharacter) == -1)
						);
		}

		/// <summary>
		/// Test to successfully parse messages from IBM Domino with "phantom sections".
		/// </summary>
		[TestMethod]
		public void TestReadMailWithPhantomSections()
		{
			LoadAndAssertMessage("4_Phantom.emr");
		}

		private static void AssertMessageSubjectEqual(string fileName, string subject)
		{
			LoadAndAssertMessage(
							fileName,
							new MessageValidateParameters { Subject = true },
							msg => msg.Subject.Equals(subject, StringComparison.Ordinal)
						);
		}

		private static void AssertAttachmentNames(string fileName, params string[] attachmentNames)
		{
			LoadAndAssertMessage(
							fileName,
							MessageValidateParameters.GetAttachments(attachmentNames.Length),
							msg => CollectionAssert.AreEqual(attachmentNames, msg.Attachments.Select(a => a.Name).ToArray())
						);
		}
	}
}
