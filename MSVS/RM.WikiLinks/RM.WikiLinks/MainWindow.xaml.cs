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
			ButtonDo.Click += OnButtonDoClick;
		}

		private async void OnButtonDoClick(object sender, RoutedEventArgs routedEventArgs)
		{
			var prov = new WikiPageLinkProvider();
			var oldText = ButtonDo.Content;
			var startTerm = TextBoxStartTerm.Text;
			var endTerm = TextBoxEndTerm.Text;

			ButtonDo.Content = "DOING...";
			SetControlsEnabled(false, ButtonDo, TextBoxStartTerm, TextBoxEndTerm);

			DataContext = await GetLinkChainAsync(prov, startTerm, endTerm);

			ButtonDo.Content = oldText;
			SetControlsEnabled(true, ButtonDo, TextBoxStartTerm, TextBoxEndTerm);
		}

		private static void SetControlsEnabled(bool enabled, params UIElement[] controls)
		{
			foreach (var control in controls)
			{
				control.IsEnabled = enabled;
			}
		}

		private static Task<string[]> GetLinkChainAsync(WikiPageLinkProvider provider, string beginTerm, string endTerm)
		{
			return Task.Run(() => provider.GetWikiLinkPath(beginTerm, endTerm));
		}
	}
}
