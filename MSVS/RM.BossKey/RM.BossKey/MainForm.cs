using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using RM.BossKey.Components;

namespace RM.BossKey
{
	public partial class MainForm : Form
	{
		private readonly Hotkey _hotkey;
		private ShowHideNativeWindow _shWindow;
		private int _hkID;

		public MainForm()
		{
			InitializeComponent();
			InitializeResources();

			_hotkey = new Hotkey();
			_hotkey.Press += OnHotkeyPress;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			e.Cancel = true;
			Hide();
		}


		private void InitializeResources()
		{
			Icon = Properties.Resources.Main;
			notifyIcon.Icon = Icon;
		}

		private void OnHotkeyPress(object sender, EventArgs<int> e)
		{
			_shWindow?.ToggleVisibility();
		}

		private void showToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Visible = !Visible;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_hotkey.Press -= OnHotkeyPress;
			_hotkey.Dispose();
			
			Application.Exit();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				_hkID = _hotkey.Register(Modifiers.Win, Keys.NumPad0);
				_shWindow = ShowHideNativeWindow.Find("TscShellContainerClass", null);
				labelIndi.BackColor = Color.Green;
			}
			catch (Win32Exception exc)
			{
				MessageBox.Show(exc.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (_shWindow != null)
			{
				_shWindow.ReleaseHandle();
				_shWindow = null;
			}

			if (_hotkey.Unregister(_hkID))
			{
				_hkID = -1;
				labelIndi.BackColor = Color.Red;
			}
		}
	}
}
