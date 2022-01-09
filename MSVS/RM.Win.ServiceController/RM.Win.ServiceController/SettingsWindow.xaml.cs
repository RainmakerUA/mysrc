using System.Windows;

namespace RM.Win.ServiceController
{
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		public SettingsWindow()
		{
			InitializeComponent();
		}

		private void OnSaveClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
