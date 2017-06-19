using System;
using System.Windows.Forms;
using ServiceController.Controls;
using ServiceController.Helpers;
using ServiceController.Settings;

namespace ServiceController.Forms
{
	public sealed partial class MainForm : Form
	{
		private Settings.Settings _settings;

		public MainForm()
		{
			InitializeComponent();
			pictureBoxLogo.Image = Icon.ToBitmap();

			string ver = Helper.GetAssemblyVersion();
			if (!String.IsNullOrEmpty(ver))
			{
				Text += String.Concat(" - Version ", ver);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SettingsManager.Current.SettingsChanged += OnSettingsChanged;
			OnSettingsChanged(this, new EventArgs<Settings.Settings>(SettingsManager.Current.Settings));
		}

		private void LoadServices(string[] services)
		{
			panelList.Controls.Clear();

			if (services != null)
			{
				for (int i = services.Length - 1; i >= 0; --i)
				{
					panelList.Controls.Add(new ServiceControl(services[i]));
				}
			}
		}

		private void SetUpdateIntervals(int interval)
		{
			ForEachService(
					(sc, @int) => sc.SetUpdateInterval(@int),
					interval
				);
		}

		private void ForEachService(Action<ServiceControl> action)
		{
			if (action == null)
			{
				return;
			}

			foreach (Control control in panelList.Controls)
			{
				var sc = control as ServiceControl;
				if (sc != null)
				{
					action(sc);
				}
			}
		}

		private void ForEachService<T>(Action<ServiceControl, T> action, T arg)
		{
			if (action == null)
			{
				return;
			}

			foreach (Control control in panelList.Controls)
			{
				var sc = control as ServiceControl;
				if (sc != null)
				{
					action(sc, arg);
				}
			}
		}

		private void OnSettingsChanged(object sender, EventArgs<Settings.Settings> e)
		{
			_settings = SettingsManager.Current.Settings;

			SuspendLayout();

			if (!_settings.FormLocation.IsEmpty)
			{
				Location = _settings.FormLocation;
			}

			if (!_settings.FormSize.IsEmpty)
			{
				Size = _settings.FormSize;
			}

			LoadServices(_settings.Services);
			SetUpdateIntervals(_settings.UpdateInterval);

			ResumeLayout(true);
		}

		private void OnButtonSettingsClick(object sender, EventArgs e)
		{
			FormSettings.Open(this);
		}

		private void OnButtonMinClick(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}

		private void OnButtonStartClick(object sender, EventArgs e)
		{
			ForEachService(
					sc => sc.StartService()
				);
		}

		private void OnButtonStopClick(object sender, EventArgs e)
		{
			ForEachService(
					sc => sc.StopService()
				);
		}

		private void OnButtonRestartClick(object sender, EventArgs e)
		{
			System.Threading.ThreadPool.QueueUserWorkItem(
					obj => ForEachService(sc => sc.RestartService())
				);
		}
	}
}