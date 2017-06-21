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

		public static Window ShowSelectDialog(WpfWindow owner)
		{
			var viewModel = new SelectViewModel();
			var win = new SelectWindow { DataContext = viewModel, Owner = owner };

			viewModel.LoadWindows();
			return win.ShowDialog().GetValueOrDefault() ? viewModel.SelectedWindow : null;
		}
	}
}
