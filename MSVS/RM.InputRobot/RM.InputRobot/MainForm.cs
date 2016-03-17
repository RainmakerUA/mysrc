using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RM.InputRobot.Modules;

namespace RM.InputRobot
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			var input = new Input();
			input.TestInput();
		}
	}
}
