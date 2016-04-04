using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using RM.WP.GpsMonitor.DataProviders;
using RM.WP.GpsMonitor.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace RM.WP.GpsMonitor
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			ViewModel = new SettingsViewModel();

			InitializeComponent();
		}

		public SettingsViewModel ViewModel { get; }

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}
	}
}
