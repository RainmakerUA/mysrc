using RM.Win.BossKey.ViewModel;
using WpfWindow = System.Windows.Window;

namespace RM.Win.BossKey
{
	/// <summary>
	/// Interaction logic for SelectWindow.xaml
	/// </summary>
	public partial class SelectWindow : WpfWindow
	{
		public SelectWindow()
		{
			InitializeComponent();
		}

		public SelectViewModel ViewModel => DataContext as SelectViewModel;

		public static Window ShowSelectDialog(WpfWindow owner)
		{
			var win = new SelectWindow { Owner = owner };
			var viewModel = win.ViewModel;

			viewModel.LoadWindows();

			return win.ShowDialog().GetValueOrDefault() ? viewModel.SelectedWindow : null;
		}
	}
}
