using System.Text.RegularExpressions;

namespace RM.UzTicket.Lib.Utils
{
	internal static class TokenParser
	{
		private static readonly Regex _jjPattern = new Regex(@";_gaq.push\(\['_trackPageview'\]\);(.+)\(function", RegexOptions.CultureInvariant);
		private static readonly Regex _tokenPattern = new Regex("localStorage.setItem\\(\"gv-token\", \"(\\w+)\"\\);", RegexOptions.CultureInvariant);

		public static string ParseGvToken(string page)
		{
			var jjCodeMatch = _jjPattern.Match(page);

			if (jjCodeMatch.Success)
			{
				var jjCode = jjCodeMatch.Groups[1].Value;
				var decoded = JjDecoder.Decode(jjCode);
				var tokenMatch = _tokenPattern.Match(decoded);

				if (tokenMatch.Success)
				{
					return tokenMatch.Groups[1].Value;
				}
			}

			return null;
		}
	}
}
