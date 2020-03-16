using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace RM.Win.MonitorPower.Interop
{
	internal static partial class Win32
	{
		public static IReadOnlyList<PhysicalMonitor> GetPhysicalMonitors()
		{
			var physMonitors = new HashSet<PhysicalMonitor>();
			var gcHandle = GCHandle.Alloc(physMonitors, GCHandleType.Normal);

			try
			{
				var ptrHandle = GCHandle.ToIntPtr(gcHandle);

				if (NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnum, ptrHandle))
				{
					var result = new PhysicalMonitor[physMonitors.Count];
					physMonitors.CopyTo(result);
					return result;
				}

				return Array.Empty<PhysicalMonitor>();
			}
			finally
			{
				gcHandle.Free();
			}
		}

		public static void SetMonitorPower(SafeHandle physicalMonitor, uint powerState)
		{
			const byte monitorPower = 0xD6;

			if (!NativeMethods.SetVCPFeature(physicalMonitor, monitorPower, powerState))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		private static bool MonitorEnum([In] IntPtr hMonitor, [In] IntPtr hDC, [In] IntPtr lpRect, [In] IntPtr lParam)
		{
			if (NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out var numMonitors))
			{
				var monitors = new NativeMethods.PhysicalMonitor[numMonitors];

				if (NativeMethods.GetPhysicalMonitorsFromHMONITOR(hMonitor, numMonitors, monitors))
				{
					var physMonitors = (HashSet<PhysicalMonitor>) GCHandle.FromIntPtr(lParam).Target;

					foreach (var monitor in monitors)
					{
						physMonitors.Add(new PhysicalMonitor(
												new HPhysicalMonitor(monitor.Handle, true),
												monitor.Description?.Trim() ?? $"({monitor.Handle}) No description"
											));
					}
				}
			}

			return true;
		}

		private static string GetStringFromCharArray(char[] chars)
		{
			return chars != null && chars.Length > 0 ? new String(chars) : null;
		}
	}
}
