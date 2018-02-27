using System;

namespace Matrix42.Client.Mail.Console
{
	internal sealed class MimeTest
	{
		public void Execute()
		{
			bool isMimeMode = false;
			bool exitRequest;

			do
			{
				System.Console.Write(
									isMimeMode
										? "Enter MIME type (--- to switch to extension mode, none to finish): "
										: "Enter extension (--- to switch to mime mode, none to finish): "
								);

				var entry = System.Console.ReadLine();
				exitRequest = String.IsNullOrEmpty(entry);

				if (!exitRequest)
				{
					if (entry.Equals("---", StringComparison.OrdinalIgnoreCase))
					{
						isMimeMode = !isMimeMode;
					}
					else
					{
						var result = isMimeMode
											? Utility.MimeTypeHelper.GetExtensions(entry)
											: Utility.MimeTypeHelper.GetMimeTypes(entry);
						System.Console.WriteLine("Results: {0}", result == null || result.Count == 0 ? "<none>" : String.Join(", ", result));
					}
				}
			}
			while (!exitRequest);
		}
	}
}
