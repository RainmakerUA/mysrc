using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ServiceController.Helpers;
using ServiceController.Settings;

namespace ServiceController.Forms
{
	public sealed partial class FormSettings : Form
	{
		/*
			W3SVC
			update4u.SPS.Engines.Common
			update4u.SPS.Engines.CommonX86
			update4u.SPS.Engines.Scheduler
		*/

		private const string _locationFormat = "{0:###} ; {1:###}";
		private const string _sizeFormat = "{0:###} x {1:###}";

		private static readonly Dictionary<string, int> _updates;

		private Form _parent;
		private Settings.Settings _settings;

		static FormSettings()
		{
			_updates = new Dictionary<string, int>();
			_updates.Add("Low (5s)", 5000);
			_updates.Add("Normal (1s)", 1000);
			_updates.Add("High (500ms)", 500);
			_updates.Add("Very high (100ms)", 100);
		}

		private FormSettings()
		{
			InitializeComponent();
		}

		private void FillUpdateComboBox()
		{
			comboBoxUpdateRate.Items.Clear();
			foreach (string key in _updates.Keys)
			{
				comboBoxUpdateRate.Items.Add(key);
			}

			comboBoxUpdateRate.SelectedIndex = 0;
			foreach (KeyValuePair<string, int> pair in _updates)
			{
				if (pair.Value == _settings.UpdateInterval)
				{
					comboBoxUpdateRate.SelectedItem = pair.Key;
					break;
				}
			}
		}

		private void GetFormLocationSize(Form form)
		{
			_settings.FormLocation = form.Location;
			_settings.FormSize = form.Size;
			UpdateLocationSizeText();
		}

		private void UpdateLocationSizeText()
		{
			labelPos.Text = String.Format(_locationFormat, _settings.FormLocation.X, _settings.FormLocation.Y);
			labelSize.Text = String.Format(_sizeFormat, _settings.FormSize.Width, _settings.FormSize.Height);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (_settings == null)
			{
				_settings = new Settings.Settings();
			}
			textBoxServices.Lines = _settings.Services ?? new string[0];
			FillUpdateComboBox();
			GetFormLocationSize(_parent);
		}

		protected override void OnClosed(EventArgs e)
		{
			_settings.Services = Array.FindAll(
					textBoxServices.Lines,
					Predicates.And(
							Predicates.Not<string>(String.IsNullOrEmpty),
							delegate(string s)
							{
								return s.Trim().Length > 0;
							}
						)

				);
			_settings.UpdateInterval = _updates[(string)comboBoxUpdateRate.SelectedItem];

			base.OnClosed(e);
		}

		public static void Open(Form parent)
		{
			FormSettings fs = new FormSettings();
			fs._parent = parent;
			fs._settings = SettingsManager.Current.Settings;

			DialogResult result = fs.ShowDialog(parent);

			if (result == DialogResult.Yes || result == DialogResult.OK)
			{
				SettingsManager.Current.Settings = fs._settings;
				if (result == DialogResult.Yes)
				{
					SettingsManager.Current.Save();
				}
			}
		}

		private void OnButtonCurrentSizeClick(object sender, EventArgs e)
		{
			GetFormLocationSize(_parent);
		}

		private void OnButtonChooseSizeClick(object sender, EventArgs e)
		{
			FormPosition fpos = new FormPosition();

			fpos.Location = _settings.FormLocation;
			fpos.Size = _settings.FormSize;

			fpos.MinimumSize = _parent.MinimumSize;
			fpos.MaximumSize = _parent.MaximumSize;

			if (fpos.ShowDialog(this) == DialogResult.OK)
			{
				GetFormLocationSize(fpos);
			}
		}
	}
}