using System;
using System.Windows.Input;
using Windows.UI.Popups;
using RM.WP.GpsMonitor.Common;
using RM.WP.GpsMonitor.Settings;

namespace RM.WP.GpsMonitor.ViewModels
{
	public class SettingsViewModel : ObservableObject
	{
		internal SettingsViewModel()
		{
			Settings = AppSettings.Instance;
			ResetCommand = new RelayCommand(ResetSettings);
		}

		public string PageTitle => ResourceHelper.GetString("SettingsName").ToUpper();

		public AppSettings Settings { get; }

		public ICommand ResetCommand { get; }

		private async void ResetSettings()
		{
			var dlg = new MessageDialog("Reset?", PageTitle);
			dlg.Commands.Add(new UICommand("Do it!") {Id = new object()});
			dlg.Commands.Add(new UICommand("Forget it!"));
			dlg.DefaultCommandIndex = 0;
			dlg.CancelCommandIndex = 1;
			
			var res = await dlg.ShowAsync();
			if (res.Id != null)
			{
				Settings.Provider.Reset();
			}
		}
	}
}
