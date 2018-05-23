using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Matrix42.Client.Mail.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Matrix42.Client.Mail.Test.Imap
{
	public abstract class ImapLoadMessageTestBase
	{
		protected class MessageValidateParameters
		{
			public bool Attachments { get; set; }

			public bool BccRecipients { get; set; }

			public bool BodyHtml { get; set; }

			public bool BodyText { get; set; }

			public bool CcRecipients { get; set; }

			public bool From { get; set; }

			public bool ReceiveDate { get; set; }

			public bool Sender { get; set; }

			public bool Subject { get; set; }

			public bool ToRecipients { get; set; }

			public static MessageValidateParameters GetDefault() => new MessageValidateParameters
																		{
																			From = true,
																			ToRecipients = true,
																			Subject = true,
																			BodyText = true,
																			ReceiveDate = true
																		};
		}

		protected const char InvalidCharacter = '\uFFFD';

		protected static IMessage GetMessage(string filename)
		{
			if (String.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentException("Filename cannot be empty, null or only whitespaces", nameof(filename));
			}
			
			using (var client = MailClientFactory.GetClient(MailServerType.Imap4))
			{
				return client.LoadMessage(Path.Combine("Imap", filename), "-1");
			}
		}

		protected static void LoadAndAssertMessage(string filename, MessageValidateParameters parameters = null)
		{
			var message = GetMessage(filename);
			AssertMessage(message, parameters ?? MessageValidateParameters.GetDefault());
		}

		private static void AssertMessage(IMessage message, MessageValidateParameters parameters)
		{
			Assert.IsNotNull(message);

			if (parameters.Attachments)
			{
				Assert.AreNotEqual(0, message.Attachments.Count);
			}

			if (parameters.BccRecipients)
			{
				AssertAddressesNotEmpty(message.Bcc);
			}

			if (parameters.BodyHtml)
			{
				AssertStringIsNotNullOrEmpty(message.BodyHtml);
			}

			if (parameters.BodyText)
			{
				AssertStringIsNotNullOrEmpty(message.BodyText);
			}

			if (parameters.CcRecipients)
			{
				AssertAddressesNotEmpty(message.Cc);
			}

			if (parameters.From)
			{
				AssertAddressNotEmpty(message.From);
			}

			if (parameters.ReceiveDate)
			{
				Assert.AreNotEqual(DateTime.MinValue, message.ReceivedDate.GetValueOrDefault());
			}

			if (parameters.Sender)
			{
				AssertAddressNotEmpty(message.Sender);
			}

			if (parameters.Subject)
			{
				AssertStringIsNotNullOrEmpty(message.Subject);
			}

			if (parameters.ToRecipients)
			{
				AssertAddressesNotEmpty(message.To);
			}
		}

		private static void AssertStringIsNotNullOrEmpty(string str)
		{
			Assert.IsFalse(String.IsNullOrEmpty(str));
		}

		private static void AssertAddressNotEmpty(MailAddress address)
		{
			Assert.IsFalse(AddressIsEmpty(address));
		}

		private static void AssertAddressesNotEmpty(IList<MailAddress> addresses)
		{
			Assert.AreNotEqual(0, addresses.Count);
			Assert.IsFalse(addresses.Any(AddressIsEmpty));
		}

		private static bool AddressIsEmpty(MailAddress address)
		{
			return String.IsNullOrEmpty(address?.Address) || String.IsNullOrEmpty(address.DisplayName);
		}
	}
}
