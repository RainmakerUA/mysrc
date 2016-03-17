using System;
using System.Runtime.InteropServices;

namespace RM.InputRobot.Native
{
	internal static class Win32
	{
		private static class NativeMethods
		{
			private const string _user32 = "user32.dll";

			[DllImport(_user32)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool SetForegroundWindow(IntPtr hWnd);

			[DllImport(_user32)]
			public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

			[DllImport(_user32, SetLastError = true)]
			public static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] inputs, int sizeOfInputStructure);
		}

		public static void SetForegroundWindow(IntPtr hWnd)
		{
			NativeMethods.SetForegroundWindow(hWnd);
		}

		public static void ShowWindow(IntPtr hWnd, SW cmd)
		{
			NativeMethods.ShowWindow(hWnd, (int)cmd);
		}

		public static int SendInput(INPUT[] inputs)
		{
			var numberOfInputs = (uint)inputs.Length;
			return numberOfInputs == NativeMethods.SendInput(numberOfInputs, inputs, INPUT.Size)
					? 0
					: Marshal.GetLastWin32Error();
		}
	}
}
