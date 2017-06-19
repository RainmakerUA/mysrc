using Con = System.Console;

namespace Matrix42.Client.Mail.Console
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var parserTest = new ParserTest();
			parserTest.Execute();

			var imapTest = new ImapClientTest();
			imapTest.Execute();

			Con.Write("Press any key...");
			Con.ReadKey(true);
		}
	}
}
