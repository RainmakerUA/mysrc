using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RM.WikiLinks.Providers;

namespace RM.WikiLinks
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			ButtonDo.Click+= OnButtonDoClick;
		}

		private async void OnButtonDoClick(object sender, RoutedEventArgs routedEventArgs)
		{
			var btn = sender as Button;
			var prov = new WikiPageLinkProvider();
			var oldText = btn.Content;

			btn.Content = "DOING...";
			btn.IsEnabled = false;

			var sr = await prov.GetWikiSearch("Пуле", 20);
			DataContext = await GetLinkChainAsync(prov, "Игла", "Топор");

			btn.Content = oldText;
			btn.IsEnabled = true;
		}

		private static Task<string[]> GetLinkChainAsync(WikiPageLinkProvider provider, string beginTerm, string endTerm)
		{
			return Task.Run(() => provider.GetWikiLinkPath(beginTerm, endTerm));
		}
	}
}
