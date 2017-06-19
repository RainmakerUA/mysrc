using Con = System.Console;

namespace Matrix42.Client.Mail.Console
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var parserTest = new ParserTest();
			parserTest.Execute();

			var imapTest = new ExchangeClientTest();
			imapTest.Execute();

			//var mimeTest = new MimeTest();
			//mimeTest.Execute();

			Con.Write("Press any key...");
			Con.ReadKey(true);
		}
	}
}
