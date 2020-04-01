using System.ComponentModel;
using System.Runtime.InteropServices;

namespace RM.Win.FlashNotifier.Interop
{
	internal static partial class Win32
	{
		private const string _shellHookMessageString = "SHELLHOOK";

		public const int HSHELL_FLASH = 0x8000 | 6;

		public static readonly uint WM_SHELLHOOKMESSAGE;

		static Win32()
		{
			WM_SHELLHOOKMESSAGE = NativeMethods.RegisterWindowMessage(_shellHookMessageString);

			if (WM_SHELLHOOKMESSAGE == 0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}
	}
}
