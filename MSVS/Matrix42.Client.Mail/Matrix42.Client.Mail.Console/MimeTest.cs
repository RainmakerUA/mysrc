using System;
using System.Linq;

namespace Matrix42.Client.Mail.Console
{
	internal sealed class MimeTest
	{
		public void Execute()
		{
			string entry = null;
			bool isMimeMode = false;

			do
			{
				System.Console.Write(
									isMimeMode
										? "Enter MIME type (--- to switch to extension mode, none to finish): "
										: "Enter extension (--- to switch to mime mode, none to finish): "
								);
				entry = System.Console.ReadLine();

				if (!String.IsNullOrEmpty(entry))
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
						System.Console.WriteLine("Results: {0}", result == null ? "<none>" : String.Join(", ", result));
					}
				}
			}
			while (!String.IsNullOrEmpty(entry));
		}
	}
}
