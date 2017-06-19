using System;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using ServiceController.Helpers;

namespace ServiceController.Controls
{
	public sealed partial class ServiceControl : UserControl
	{
		private const string _notFound = "(not found)";

		private readonly System.ServiceProcess.ServiceController _service;

		private ServiceControl()
		{
			InitializeComponent();
			Dock = DockStyle.Top;
		}

		public ServiceControl(string serviceName)
			: this()
		{
			try
			{
				_service = new System.ServiceProcess.ServiceController(serviceName);
				string dn = _service.DisplayName;
			}
			catch (InvalidOperationException)
			{
				_service = null;
			}

			timerUpdate.Enabled = true;
			UpdateUI();
		}

		public void StartService(bool sync = false)
		{
			RefreshService();

			if (CanStartService(_service))
			{
				_service.Start();
				if (sync)
				{
					_service.WaitForStatus(ServiceControllerStatus.Running);
				}
			}
		}

		public void SetUpdateInterval(int interval)
		{
			if (interval == 0)
			{
				return;
			}

			timerUpdate.Stop();
			timerUpdate.Interval = interval;
			timerUpdate.Start();
		}

		public void StopService(bool sync = false)
		{
			RefreshService();

			if (CanStopService(_service))
			{
				_service.Stop();
				if (sync)
				{
					_service.WaitForStatus(ServiceControllerStatus.Stopped);
				}
			}
		}

		public void RestartService()
		{
			StopService(true);
			StartService(true);
		}

		private void RefreshService()
		{
			if (_service != null)
			{
				try
				{
					_service.Refresh();
				}
				catch (Exception)
				{
				}
			}
		}

		private bool CanStartService(System.ServiceProcess.ServiceController service)
		{
			if (!checkBoxEnabled.Checked || service == null)
			{
				return false;
			}

			ServiceControllerStatus status = service.Status;
			return status != ServiceControllerStatus.Running &&
					status != ServiceControllerStatus.StartPending &&
					status != ServiceControllerStatus.ContinuePending;
		}

		private bool CanStopService(System.ServiceProcess.ServiceController service)
		{
			if (!checkBoxEnabled.Checked || service == null)
			{
				return false;
			}

			ServiceControllerStatus status = service.Status;
			return service.CanStop &&
					status != ServiceControllerStatus.Stopped &&
					status != ServiceControllerStatus.StopPending;
		}

		private void UpdateUI()
		{
			string serviceDesc = _notFound;
			string serviceState = String.Empty;
			bool canStart = false;
			bool canStop = false;

			if (_service != null)
			{
				try
				{
					ServiceControllerStatus status = _service.Status;
					serviceState = Helper.GetEnumText(status);
				}
				catch (Exception)
				{
					serviceState = "Error!";
				}

				try
				{
					serviceDesc = _service.DisplayName;
				}
				catch (Exception)
				{
					serviceDesc = "Error!";
				}

				try
				{
					canStart = CanStartService(_service);
				}
				catch (Exception)
				{
					canStart = false;
				}

				try
				{
					canStop = CanStopService(_service);
				}
				catch (Exception)
				{
					canStart = false;
				}
			}

			labelFullName.Text = serviceDesc;
			labelStatus.Text = serviceState;

			buttonStart.Enabled = canStart;
			buttonStop.Enabled = canStop;
			buttonRestart.Enabled = canStart || canStop;
		}

		//private void ObCheckBoxEnabler

		private void OnTimerUpdateTick(object sender, EventArgs e)
		{
			RefreshService();
			UpdateUI();
		}

		private void OnButtonStartClick(object sender, EventArgs e)
		{
			StartService();
		}

		private void OnButtonStopClick(object sender, EventArgs e)
		{
			StopService();
		}

		private void OnButtonRestartClick(object sender, EventArgs e)
		{
			ThreadPool.QueueUserWorkItem(obj => RestartService());
		}
	}
}
