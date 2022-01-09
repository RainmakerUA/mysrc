using RM.Lib.Common.Settings;

namespace RM.Win.ServiceController.Settings
{
	public class AppSettings: UserSettings, IUseLocalAppData
	{
		public bool UseLocalAppData { get; set; }

		public new AppSettings Clone()
		{
			var clone = (MemberwiseClone() as AppSettings)!;

			clone.Geometry = Geometry.Clone();

			return clone;
		}
	}
}
