using System.Collections.Generic;

namespace RM.Win.ServiceController.Model
{
	public class SettingsModel
	{
		public class LanguageInfo
		{
			public LanguageInfo(string name, int lcid)
			{
				Name = name;
				Lcid = lcid;
			}

			public string Name { get; }

			public int Lcid { get; }
		}

		public class RefreshRateInfo
		{
			public RefreshRateInfo(string name, ushort rate)
			{
				Name = name;
				Rate = rate;
			}

			public string Name { get; }

			public ushort Rate { get; }
		}

		public SettingsModel(IReadOnlyList<LanguageInfo> languages, IReadOnlyList<RefreshRateInfo> refreshRates)
		{
			SupportedLanguages = languages;
			RefreshRates = refreshRates;
		}

		public IReadOnlyList<LanguageInfo> SupportedLanguages { get; }

		public IReadOnlyList<RefreshRateInfo> RefreshRates { get; }

		public int Lcid { get; set; }

		public ushort RefreshInterval { get; set; }

		public bool Autostart { get; set; }

		public string? Services { get; set; }
	}
}
