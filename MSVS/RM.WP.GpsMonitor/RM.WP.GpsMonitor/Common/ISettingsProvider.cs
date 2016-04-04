using System.Runtime.CompilerServices;

namespace RM.WP.GpsMonitor.Common
{
	public interface ISettingsProvider
	{
		//void Load();
		
		//void Save();

		void Reset();
		
		T Get<T>([CallerMemberName] string key = null);
		
		void Set<T>(T value, [CallerMemberName] string key = null);

		void Set<T>(string key, T value);
	}
}
