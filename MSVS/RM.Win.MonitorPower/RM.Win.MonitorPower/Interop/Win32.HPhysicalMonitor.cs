using System;
using System.Runtime.InteropServices;

namespace RM.Win.MonitorPower.Interop
{
	internal static partial class Win32
	{
		private sealed class HPhysicalMonitor : SafeHandle
		{
			public HPhysicalMonitor(IntPtr existingHandle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle)
			{
				SetHandle(existingHandle);
			}

			protected override bool ReleaseHandle()
			{
				return NativeMethods.DestroyPhysicalMonitor(handle);
			}

			public override bool IsInvalid => IsClosed || handle == IntPtr.Zero;
		}
	}
}
