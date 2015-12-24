using System;
using System.Collections.Generic;
using System.IO;

namespace RM.Shooter.Configuration
{
	internal partial class IniFile : IKeyedStorage<string, IKeyedStorage<string, string>>
	{
		private readonly string _fileName;
		private readonly IDictionary<string, IniSection> _sections;

		public IniFile(string fileName)
		{
			_fileName = Path.GetFullPath(fileName);
			_sections = new Dictionary<string, IniSection>();
		}

		#region Implementation of IKeyedStorage<string,IKeyedStorage<string,string>>

		public IEnumerable<string> Keys
		{
			get
			{
				return Native.GetIniString(_fileName, null, null)
								.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
			}
		}

		public IKeyedStorage<string, string> this[string key]
		{
			get { return GetSection(key); }
			set { throw new NotImplementedException("Cannot set sections!"); }
		}

		#endregion

		private IniSection GetSection(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			if (!_sections.ContainsKey(name))
			{
				_sections.Add(name, new IniSection(_fileName, name));
			}

			return _sections[name];
		}
	}
}
