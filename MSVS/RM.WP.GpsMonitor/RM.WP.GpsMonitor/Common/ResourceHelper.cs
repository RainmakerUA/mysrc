using Windows.ApplicationModel.Resources;

namespace RM.WP.GpsMonitor.Common
{
	public static class ResourceHelper
	{
		private const string _noValue = "NoValueString";
		private static readonly ResourceLoader _loader = new ResourceLoader();

		public static string NoValue
		{
			get { return GetString(_noValue); }
		}

		public static string GetString(string key)
		{
			return _loader.GetString(key);
		}
	}
}
