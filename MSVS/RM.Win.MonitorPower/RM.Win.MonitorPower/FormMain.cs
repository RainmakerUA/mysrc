using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace RM.Win.MonitorPower
{
	public partial class FormMain : Form
	{
		private IReadOnlyList<PhysicalMonitor> _monitors;

		public FormMain()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			OnButtonRefreshClick(this, EventArgs.Empty);
		}

		private void OnButtonRefreshClick(object sender, EventArgs e)
		{
			try
			{
				_monitors = PhysicalMonitor.GetPhysicalMonitors();
			}
			catch (Exception)
			{
				_monitors = Array.Empty<PhysicalMonitor>();
			}

			comboBoxMonitors.Items.Clear();

			if (_monitors.Count > 0)
			{
				foreach (var monitor in _monitors)
				{
					comboBoxMonitors.Items.Add(monitor.Description);
				}
			}
			else
			{
				comboBoxMonitors.Items.Add("(no monitors detected)");
			}

			comboBoxMonitors.SelectedIndex = 0;
		}

		private void OnButtonOnClick(object sender, EventArgs e)
		{
			SetMonitorPowerByIndex(comboBoxMonitors.SelectedIndex, MonitorPowerState.On);
		}

		private void OnButtonOffClick(object sender, EventArgs e)
		{
			SetMonitorPowerByIndex(comboBoxMonitors.SelectedIndex, MonitorPowerState.Off);
		}

		private void SetMonitorPowerByIndex(int index, MonitorPowerState powerState)
		{
			if (index >= 0 && index < _monitors.Count)
			{
				try
				{
					_monitors[index].SetPowerState(powerState);
				}
				catch (Win32Exception ex)
				{
					MessageBox.Show(
							$"Error setting monitor power\r\n{ex.NativeErrorCode}: {ex.Message}",
							Text,
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
					);
				}
			}
		}
	}
}
