
namespace RM.Lib.Common.Settings
{
	public interface ISettingsProvider<TUser, out TApp>
	{
		TApp GetApplicationSettings();

		TUser GetUserSettings();

		void SaveUserSettings(TUser settings);
	}
}
