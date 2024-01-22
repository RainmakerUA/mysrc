using System;
using System.Collections.Generic;

namespace RM.Win.ServiceController.Settings
{
	public class UserSettings : ICloneable
	{
		public UserSettings()
		{
			Services = new Dictionary<string, bool>();
			Geometry = new Geometry();
		}

		public UserSettings(UserSettings other) : this()
		{
			LaunchAtStartup = other.LaunchAtStartup;
			Language = other.Language;
			RefreshInterval = other.RefreshInterval;
			Geometry = other.Geometry.Clone();

			foreach (var (name, enabled) in other.Services)
			{
				Services.Add(name, enabled);
			}
		}

		public bool LaunchAtStartup { get; set; }

		public bool RegisterHotkeys { get; set; }

		public string? Language { get; set; }

		public IDictionary<string, bool> Services { get; set; }

		public ushort RefreshInterval { get; set; }

		public Geometry Geometry { get; set; }

		public object Clone() => new UserSettings(this);
	}
}
