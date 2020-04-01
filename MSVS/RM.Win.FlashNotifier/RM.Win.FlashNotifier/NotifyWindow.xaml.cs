using System;
using System.Windows;
using System.Windows.Input;

namespace RM.Win.FlashNotifier
{
	/// <summary>
	/// Interaction logic for NotifyWindow.xaml
	/// </summary>
	public partial class NotifyWindow : Window
	{
		public NotifyWindow()
		{
			InitializeComponent();
		}

		public bool Visible
		{
			get => Visibility == Visibility.Visible;
			set => Visibility = value ? Visibility.Visible : Visibility.Hidden;
		}

		public event EventHandler? WindowClicked;

		public string? WindowTitle
		{
			get => TextBlockWindowTitle.Text;
			set => TextBlockWindowTitle.Text = value;
		}

		public string? WindowClass
		{
			get => TextBlockWindowClass.Text;
			set => TextBlockWindowClass.Text = value;
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);

			if (!e.Handled)
			{
				WindowClicked?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
