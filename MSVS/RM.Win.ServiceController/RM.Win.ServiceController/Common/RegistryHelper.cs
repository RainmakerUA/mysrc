using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using RM.Lib.Common;

namespace RM.Win.ServiceController.Common
{
	internal static class RegistryHelper
	{
		private const string _dll = ".dll";
		private const string _exe = ".exe";
		private const string _runSubKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
		
		public static void RegisterExecutableForStartup(bool unregister = false)
		{
			var currentUser = Registry.CurrentUser;
			var subKey = currentUser.OpenSubKey(_runSubKeyPath, true) ?? currentUser.CreateSubKey(_runSubKeyPath);
			var entryAssembly = Assembly.GetEntryAssembly();
			var exeLocation = ProcessLocation.ProcessExeLocation;

			if (String.IsNullOrEmpty(exeLocation))
			{
				throw new NotSupportedException("Cannot find application executable path");
			}

			if (Path.GetExtension(exeLocation).Equals(_dll, StringComparison.OrdinalIgnoreCase))
			{
				// If entry assembly is a DLL (for .NET `Core`), replace it with EXE
				exeLocation = Path.ChangeExtension(exeLocation, _exe);
			}

			var autostartCommand = $"\"{exeLocation}\" /auto";
			
			if (unregister)
			{
				DeleteRegistration(subKey, exeLocation);
			}
			else
			{
				var entryName = entryAssembly?.GetName().Name;

				if (entryName is null)
				{
					var caller = new StackTrace().GetFrame(1)?.GetMethod();
					entryName = caller?.DeclaringType?.Namespace
									?? nameof(RegistryHelper) + $"_{DateTime.UtcNow.Ticks:08X}";
				}

				DeleteRegistration(subKey, exeLocation);

				subKey.SetValue(entryName, autostartCommand);
			}

			static void DeleteRegistration(RegistryKey key, string path)
			{
				foreach (var valueName in key.GetValueNames())
				{
					if (key.GetValue(valueName) is string strValue
						&& strValue.Contains(path, StringComparison.OrdinalIgnoreCase))
					{
						key.DeleteValue(valueName);
						break;
					}
				}
			}
		}
	}
}
