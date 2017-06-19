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

		private void Log(string message)
		{
			if (textBoxLog.TextLength > 0)
			{
				textBoxLog.AppendText(Environment.NewLine);
			}

			textBoxLog.AppendText(message);
		}

		private void OnHotkeyPress(object sender, EventArgs<int> e)
		{
			Log($"Hotkey #${e.Value} pressed.");	

			if (_shWindow != null)
			{
				Log(
					_shWindow.ToggleVisibility()
					? String.Format("Window was {0}.", _shWindow.Visible ? "shown" : "hidden")
					: String.Format("Error when changing window state (now {0})", _shWindow.Visible ? "shown" : "hidden")
				);
			}
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

				var windowName = _shWindow?.ToString() ?? "(not found)";
				Log($"Hotkey #{_hkID} registered. Window: {windowName}.");
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
				Log($"Unbinding window ${_shWindow}.");
				_shWindow.ReleaseHandle();
				_shWindow = null;
			}

			if (_hotkey.Unregister(_hkID))
			{
				Log($"Unregistered hotkey #{_hkID}.");
				_hkID = -1;
				labelIndi.BackColor = Color.Red;
			}
		}
	}
}
