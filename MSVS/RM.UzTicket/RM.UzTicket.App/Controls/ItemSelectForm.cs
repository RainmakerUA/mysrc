using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RM.UzTicket.App.Controls
{
	public partial class ItemSelectForm : Form
	{
		public ItemSelectForm()
		{
			InitializeComponent();
		}

		public string Label
		{
			get { return labelLabel.Text; }
			set { labelLabel.Text = value; }
		}

		public IReadOnlyList<string> Items
		{
			get { return listBoxItems.Items.Cast<string>().ToArray(); }
			set
			{
				listBoxItems.Items.Clear();
				listBoxItems.Items.AddRange(value.Cast<object>().ToArray());
			}
		}

		public string SelectedItem => listBoxItems.SelectedItem as string;

		public static string SelectItem(IReadOnlyList<string> items, string label)
		{
			if (items != null && items.Count > 0)
			{
				if (items.Count > 1)
				{
					var f = new ItemSelectForm
								{
									Label = label,
									Items = items
								};

					if (f.ShowDialog() == DialogResult.OK)
					{
						return (string)f.listBoxItems.SelectedItem;
					} 
				}
				else
				{
					return items[0];
				}
			}

			return null;
		}

		private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonOk.Enabled = listBoxItems.SelectedIndex >= 0;
		}
	}
}
