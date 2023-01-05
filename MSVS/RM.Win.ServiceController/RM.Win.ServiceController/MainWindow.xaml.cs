using System;
using System.Windows;
using System.Windows.Threading;
using RM.Win.ServiceController.Model;

namespace RM.Win.ServiceController
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly Action<Window, Exception> _showErrorUnsafe = ShowErrorUnsafe;

		public MainWindow()
		{
			Model.Service.Dispatcher = Dispatcher;
			Model.Service.ErrorAction = ShowError;
			MainModel.ErrorAction = ShowError;

			InitializeComponent();
		}

		public MainModel ViewModel
		{
			get => (DataContext as MainModel)!;
			set => DataContext = value;
		}

		private void ShowError(Exception? exc)
		{
			Dispatcher.Invoke(_showErrorUnsafe, DispatcherPriority.Normal, this, exc);
		}

		private static void ShowErrorUnsafe(Window owner, Exception exc)
		{
			MessageBox.Show(owner, exc.Message, owner.Title, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
