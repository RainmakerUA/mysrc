using System.Globalization;

namespace RM.Lib.Common.Localization
{
	public static class CultureHelper
	{
		public static CultureInfo Get(int lcid) => CultureInfo.GetCultureInfo(lcid);

		public static CultureInfo Get(string nameOrCode) => CultureInfo.GetCultureInfo(nameOrCode);

		public static int GetLcid(string code) => Get(code).LCID;

		public static string GetLanguageCode(int lcid) => Get(lcid).TwoLetterISOLanguageName;

		public static string GetDisplayName(int lcid) => Get(lcid).NativeName;
	}
}
