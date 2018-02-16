using System;
using MsgReader.Outlook;

namespace Matrix42.Client.Mail.Console
{
	internal class ParseMsgTest
	{
		public void Execute()
		{
			System.Console.Write("Give me MSG file: ");

			var msgFile = System.Console.ReadLine();
			msgFile = msgFile?.Trim();

			if (!String.IsNullOrEmpty(msgFile))
			{
				using (var msg = new Storage.Message(msgFile))
				{
					
				}
			}
		}
	}
}
