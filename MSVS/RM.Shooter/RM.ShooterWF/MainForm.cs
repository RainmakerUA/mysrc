using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using RM.Shooter.Modules;
using RM.Shooter.Settings;

namespace RM.Shooter
{
	public partial class MainForm : Form
	{
		private readonly ISettings _settings;
		private readonly Logger _logger;
		private readonly FrameChanger _frameChanger;
		private readonly VtAntiAfk _antiAfk;

		public MainForm()
		{
			Application.ThreadException += OnAppThreadException;
			Application.ApplicationExit += OnAppExit;

			InitializeComponent();

			notifyIconMain.Icon =
				Icon = Properties.Resources.main;
			labelVersion.Text = "ver. " + GetType().Assembly.GetName().Version;

			_settings = GlobalSettings.GetSettings();
			_logger = new Logger("MainForm");
			_frameChanger = new FrameChanger(_settings.FrameConfigs);
			_antiAfk = new VtAntiAfk();

			RegisterHook();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (Visible)
			{
				_logger.Log(Logger.Level.Info, "Reloading settings");
				_settings.Reload();
				_frameChanger.ReInitialize(_settings.FrameConfigs);
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			_logger.Log(Logger.Level.Debug, "Hiding main form");

			Visible = false;
			e.Cancel = true;
		}

		protected override void WndProc(ref Message m)
		{
			if (_frameChanger != null && toolStripMenuItemChangeFrames.Checked)
			{
				_frameChanger.ProcessShellMessage(ref m);
			}

			base.WndProc(ref m);
		}

		private void RegisterHook()
		{
			if (_frameChanger != null)
			{
				_logger.Log(Logger.Level.Debug, "Registering shell hook");

				_frameChanger.InitializeShellHook(Handle);
				_frameChanger.ApplyWindowFrames();
			}
		}

		private void UnregisterHook()
		{
			if (_frameChanger != null)
			{
				_logger.Log(Logger.Level.Debug, "Unregistering shell hook");

				_frameChanger.UninitializeShellHook(Handle);
			}
		}

		private void MakeScreenshot()
		{
			const int tipTimeout = 1000;

			_logger.Log(Logger.Level.Info, "Making a screenshot");

			try
			{
				var now = DateTime.Now;
				var dateStr = String.Format("{0:D4}{1:D2}{2:D2}", now.Year, now.Month, now.Day);
				var timeStr = String.Format("{0:D2}{1:D2}{2:D2}", now.Hour, now.Minute, now.Second);

				var shooterConfig = _settings.ShooterConfig;
				var filename = Path.Combine(
										shooterConfig.Folder,
										String.Format(shooterConfig.NameFormat, dateStr, timeStr)
									);
				Modules.Shooter.CreateShot(ref filename, shooterConfig.Format, shooterConfig.Quality);

				notifyIconMain.ShowBalloonTip(
									tipTimeout,
									Text,
									String.Format("Screenshot is saved to\r\n'{0}'", filename),
									ToolTipIcon.Info
								);
			}
			catch (Exception exc)
			{
				_logger.Log(Logger.Level.Error, "Screen shooting error: {0}", exc.Message);
				notifyIconMain.ShowBalloonTip(
									5 * tipTimeout,
									Text,
									String.Format("Error occured:\r\n{0}", exc.Message),
									ToolTipIcon.Error
								);
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void notifyIconMain_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_logger.Log(Logger.Level.Debug, "Notify icon left-clicked");

				toolStripMenuItemShot_Click(sender, e);
			}
		}

		private void toolStripMenuItemShot_Click(object sender, EventArgs e)
		{
			MakeScreenshot();
		}

		private void toolStripMenuItemChangeFramesNow_Click(object sender, EventArgs e)
		{
			if (_frameChanger != null)
			{
				_logger.Log(Logger.Level.Debug, "Applying window frame styles");

				_frameChanger.ApplyWindowFrames();
			}
		}

		private void toolStripMenuItemAntiAfk_CheckedChanged(object sender, EventArgs e)
		{
			timerAntiAfk.Enabled = toolStripMenuItemAntiAfk.Checked;
		}

		private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
		{
			Visible = true;
		}

		private void toolStripMenuItemExit_Click(object sender, EventArgs e)
		{
			_logger.Log(Logger.Level.Debug, "Stopping the application");

			UnregisterHook();
			Application.Exit();
		}

		private void timerAntiAfk_Tick(object sender, EventArgs e)
		{
			if (_antiAfk != null)
			{
				_antiAfk.PerformActivity();
			}
		}

		private void OnAppThreadException(object sender, ThreadExceptionEventArgs e)
		{
			if (_logger != null)
			{
				_logger.Log(Logger.Level.Error, e.Exception);
			}

			MessageBox.Show(e.Exception.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			Application.Exit();
		}

		private void OnAppExit(object sender, EventArgs e)
		{
			if (_logger != null)
			{
				_logger.Log(Logger.Level.Info, "Application Exit\r\n" + new String('=', 80) + "\r\n\r\n");
			}
		}
	}
}
