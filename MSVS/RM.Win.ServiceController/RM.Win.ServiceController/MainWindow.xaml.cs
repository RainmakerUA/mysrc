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
			Dispatcher.Invoke(new Action<Exception>(ShowErrorUnsafe), DispatcherPriority.Normal, exc);
		}

		private void ShowErrorUnsafe(Exception exc)
		{
			MessageBox.Show(this, exc.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
