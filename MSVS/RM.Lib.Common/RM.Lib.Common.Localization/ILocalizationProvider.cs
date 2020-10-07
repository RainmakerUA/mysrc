
using System.Collections.Generic;

namespace RM.Lib.Common.Localization
{
	public interface ILocalizationProvider
	{
		string Key { get; }

		IReadOnlyList<int> SupportedLocales { get; }

		string GetString(string key, int lcid);
	}
}
