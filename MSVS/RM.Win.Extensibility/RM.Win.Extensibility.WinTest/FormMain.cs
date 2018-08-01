using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Win.Extensibility.WinTest
{
	public partial class FormMain : Form
	{
		private class WinTest : TestContracts.IWinTest
		{
			private readonly IWin32Window _parent;

			public WinTest(IWin32Window parent)
			{
				_parent = parent;
			}

			public void ShowMessage(string message, string title)
			{
				MessageBox.Show(_parent, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private readonly Manager _extManager;

		public FormMain()
		{
			InitializeComponent();
			
			_extManager = Manager.Instance;
			_extManager.Initialize(new WinTest(this));
		}
	}
}
