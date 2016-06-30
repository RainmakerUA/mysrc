using System;
using System.IO;
using System.Reflection;
using RM.UzTicket.Lib.Utils;

namespace RM.UzTicket.Console.Tests
{
	internal static class UtilsTest
	{
		private const string _tokenFromPage = "cdde607e069b768dd83222b63a8a0fe8";

		public static void TestParseToken()
		{
			var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var filename = Path.Combine(appDir, "Files", "booking.html");
			var contents = File.ReadAllText(filename);
			var decoded = TokenParser.ParseGvToken(contents);

			if (!_tokenFromPage.Equals(decoded, StringComparison.Ordinal))
			{
				throw new Exception($"JjDecoder test failed:\nExpected: '{_tokenFromPage}'\nActual: '{decoded}'");
			}
		}
	}
}
