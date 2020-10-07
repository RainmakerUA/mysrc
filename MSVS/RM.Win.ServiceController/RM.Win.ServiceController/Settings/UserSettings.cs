using System;
using System.Collections.Generic;

namespace RM.Win.ServiceController.Settings
{
	public class UserSettings : ICloneable
	{
		private IDictionary<string, bool> _services;
		private Geometry _geometry;

		public UserSettings()
		{
			_services = new Dictionary<string, bool>();
			Geometry = new Geometry();
		}

		public UserSettings(UserSettings other) : this()
		{
			LaunchAtStartup = other.LaunchAtStartup;
			Language = other.Language;
			RefreshInterval = other.RefreshInterval;
			Geometry = other.Geometry.Clone();

			foreach (var kv in other.Services)
			{
				_services.Add(kv.Key, kv.Value);
			}
		}

		public bool LaunchAtStartup { get; set; }

		public string Language { get; set; }

		public IDictionary<string, bool> Services
		{
			get => _services;
			set => _services = value ?? new Dictionary<string, bool>();
		}

		public ushort RefreshInterval { get; set; }

		public Geometry Geometry
		{
			get => _geometry;
			set => _geometry = value ?? new Geometry();
		}

		public object Clone()
		{
			var result = MemberwiseClone() as UserSettings;
			result.Geometry = Geometry.Clone();

			return result;
		}
	}
}
