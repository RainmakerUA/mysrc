using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RM.UzTicket.App.Controls
{
	public sealed partial class CompletableTextBox : UserControl
	{
		private static readonly Color _validColor = SystemColors.Window;
		private static readonly Color _invalidColor = Color.OrangeRed;

		public CompletableTextBox()
		{
			InitializeComponent();
		}

		[Category("Appearence")]
		[Description("Label for textbox")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public string Label
		{
			get => labelLabel.Text;
			set => labelLabel.Text = value;
		}

		[Category("Appearence")]
		[Description("Text in the textbox")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public string Value
		{
			get => textBoxText.Text;
			set => textBoxText.Text = value;
		}

		[Description("Occurs when text must be completed")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<CompleteAsyncEventArgs> Complete;

		public async void UpdateTextAsync()
		{
			var args = new CompleteAsyncEventArgs(Value);
			OnComplete(args);

			var suggestions = await args.SuggestionsAsync;
			var result = ItemSelectForm.SelectItem(suggestions, Label);

			if (result != null)
			{
				Value = result;
				textBoxText.BackColor = _validColor;
			}
			else
			{
				textBoxText.BackColor = _invalidColor;
			}
		}

		#region Private methods

		private void OnComplete(CompleteAsyncEventArgs e)
		{
			Complete?.Invoke(this, e);
		}

		#endregion

		private void buttonComplete_Click(object sender, EventArgs e)
		{
			UpdateTextAsync();
		}
	}
}
