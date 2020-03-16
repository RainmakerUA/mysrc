using System;
using System.Runtime.InteropServices;

namespace RM.Win.MonitorPower.Interop
{
	internal static partial class Win32
	{
		private static class NativeMethods
		{
			private const string _user32 = "user32.dll";
			private const string _dxva2 = "dxva2.dll";
			private const int _physicalMonitorDescriptionSize = 128;

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			public struct PhysicalMonitor
			{
				public readonly IntPtr Handle;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = _physicalMonitorDescriptionSize)]
				public readonly string Description;
			}

			[return:MarshalAs(UnmanagedType.Bool)]
			public delegate bool MonitorEnumProc([In] IntPtr hMonitor, [In] IntPtr hDC, [In] IntPtr lpRect, [In] IntPtr lParam);

			[DllImport(_user32, CharSet = CharSet.Auto)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool EnumDisplayMonitors([In] IntPtr hdc, [In] IntPtr lprcClip, [In] MonitorEnumProc lpfnEnum, [In] IntPtr dwParam);

			[DllImport(_dxva2, CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR([In] IntPtr hMonitor, [Out] out uint pdwNumberOfPhysicalMonitors);

			[DllImport(_dxva2, CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool GetPhysicalMonitorsFromHMONITOR([In] IntPtr hMonitor, [In] uint dwPhysicalMonitorArraySize, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] PhysicalMonitor[] pPhysicalMonitorArray);

			[DllImport(_dxva2, CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool SetVCPFeature([In] SafeHandle hPhysMonitor, [In] byte bCode, [In] uint dwNewValue);

			[DllImport(_dxva2, CharSet = CharSet.Auto)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DestroyPhysicalMonitor([In] IntPtr hPhysicalMonitor);
		}
	}
}
