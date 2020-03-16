using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RM.Win.MonitorPower.Interop;

namespace RM.Win.MonitorPower
{
	internal sealed class PhysicalMonitor : IDisposable
	{
		public PhysicalMonitor(SafeHandle handle, string description)
		{
			Handle = handle;
			Description = description;
		}

		public SafeHandle Handle { get; }

		public string Description { get; }

		public void SetPowerState(MonitorPowerState powerState)
		{
			Interop.Win32.SetMonitorPower(Handle, (uint)powerState);
		}

		public static IReadOnlyList<PhysicalMonitor> GetPhysicalMonitors()
		{
			return Win32.GetPhysicalMonitors();
		}

		public void Dispose()
		{
			Handle?.Dispose();
		}

		public override bool Equals(object obj)
		{
			return obj is PhysicalMonitor mon && Handle.Equals(mon.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}

		public override string ToString()
		{
			return $"Physical monitor {Handle}: {Description}";
		}
	}
}
