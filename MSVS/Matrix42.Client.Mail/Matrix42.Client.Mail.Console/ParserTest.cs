using System;
using System.IO;
using System.Linq;

namespace Matrix42.Client.Mail.Console
{
	internal class ParserTest
	{
		public void Execute()
		{
			System.Console.Write("Give me file/folder: ");

			var filename = System.Console.ReadLine();
			filename = filename?.Trim();

			if (!String.IsNullOrEmpty(filename))
			{
				IMessage msg;

				using (var client = MailClientFactory.GetClient(null, false))
				{
					msg = client.LoadMessage(filename, "-19");
				}

				System.Console.WriteLine($"Message subject: '{msg.Subject}'");
				foreach (var attachment in msg.Attachments)
				{
					System.Console.WriteLine(
									!String.IsNullOrEmpty(attachment.Name)
										? $"Attachment: '{attachment.Name}'"
										: $"Attachment (strange): '{attachment.MimeType}'"
								);
				}

				//foreach (var file in Directory.EnumerateFiles(filename).ToArray())
				//{
				//	if (!(Path.GetExtension(file)?.Equals(".emr")).GetValueOrDefault(true))
				//	{
				//		Utility.FileHelper.ConvertMessageFile(file, "19");
				//	}
				//}

				//using (var fs = File.OpenRead(filename))
				//{
				//	const int buffSize = 0x4000;
				//	var buffer = new byte[buffSize];
				//	var size = fs.Read(buffer, 0, buffSize);

				//	if (size < buffSize)
				//	{
				//		Array.Resize(ref buffer, size);
				//	}

				//	var mime = Utility.Win32Api.FindMimeFromData(buffer, null);

				//	System.Console.WriteLine("Detected MIME: {0}", mime);
				//}
			}
			else
			{
				System.Console.WriteLine("ParserTest skipped!");
			}
		}
	}
}
