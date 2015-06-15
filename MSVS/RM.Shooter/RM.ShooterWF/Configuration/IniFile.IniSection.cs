using System;
using System.Collections.Generic;

namespace RM.Shooter.Configuration
{
	partial class IniFile
	{
		private class IniSection : IKeyedStorage<string, string>
		{
			private readonly string _fileName;
			private readonly string _sectionName;

			public IniSection(string fileName, string sectionName)
			{
				_fileName = fileName;
				_sectionName = sectionName;
			}

			#region Implementation of IKeyedStorage<string,string>

			public IEnumerable<string> Keys
			{
				get
				{
					return Native.GetIniString(_fileName, _sectionName, null)
									.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
				}
			}

			public string this[string key]
			{
				get
				{
					if (key == null)
					{
						throw new ArgumentNullException("key");
					}

					return Native.GetIniString(_fileName, _sectionName, key);
				}
				set
				{
					if (key == null)
					{
						throw new ArgumentNullException("key");
					}

					Native.SetIniString(_fileName, _sectionName, key, value ?? String.Empty);
				}
			}

			#endregion
		}
	}
}
