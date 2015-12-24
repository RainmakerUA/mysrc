using System;
using System.Runtime.InteropServices;

namespace RM.Shooter.Configuration
{
	partial class IniFile
	{
		private static class Native
		{
			#region Constants

			private const string _kernel32 = "kernel32.dll";
			private const int _maxBuffer = Int16.MaxValue;

			#endregion

			#region NativeMethods

			[DllImport(_kernel32, CharSet = CharSet.Unicode)]
			private static extern uint GetPrivateProfileString(
				string lpAppName,
				string lpKeyName,
				string lpDefault,
				IntPtr lpReturnedString,
				uint nSize,
				string lpFileName);

			[DllImport(_kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool WritePrivateProfileString(
				string lpAppName,
				string lpKeyName,
				string lpString,
				string lpFileName);

			#endregion

			#region WrapperMethods

			public static string GetIniString(string fileName, string section, string key)
			{
				var result = Marshal.AllocCoTaskMem(_maxBuffer);
				try
				{
					var resultLen = GetPrivateProfileString(section, key, null, result, _maxBuffer, fileName);

					return resultLen > 0
								? Marshal.PtrToStringUni(result, (int)resultLen)
								: String.Empty;
				}
				finally
				{
					Marshal.FreeCoTaskMem(result);
				}
			}

			public static bool SetIniString(string fileName, string section, string key, string value)
			{
				return WritePrivateProfileString(section, key, value, fileName);
			}

			#endregion
		}
	}
}
