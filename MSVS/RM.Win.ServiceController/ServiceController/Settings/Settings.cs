using System;
using System.Drawing;

namespace ServiceController.Settings
{
	[Serializable]
	public class Settings
	{
		private string[] _services;
		private int _updateInterval;
		private Point _formLocation;
		private Size _formSize;

		public Settings()
		{
			_services = new string[0];
			_updateInterval = 1000;
			//
			_formSize = new Size(500, 300);
		}

		public string[] Services
		{
			get { return _services; }
			set { _services = value; }
		}

		public int UpdateInterval
		{
			get { return _updateInterval; }
			set { _updateInterval = value; }
		}

		public Point FormLocation
		{
			get { return _formLocation; }
			set { _formLocation = value; }
		}

		public Size FormSize
		{
			get { return _formSize; }
			set { _formSize = value; }
		}
	}
}
