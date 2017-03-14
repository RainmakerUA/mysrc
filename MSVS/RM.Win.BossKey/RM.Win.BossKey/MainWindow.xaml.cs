using System.Windows;

namespace RM.Win.BossKey
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void OnDoClick(object sender, RoutedEventArgs e)
		{
			var wnd = SelectWindow.ShowSelectDialog(this);

			if (wnd != null)
			{
				image.Source = wnd.Icon;
				textBlock.Text = wnd.Title;
				textBlock1.Text = wnd.Class;
				textBlock2.Text = wnd.ExeName;
			}
		}
	}
}
