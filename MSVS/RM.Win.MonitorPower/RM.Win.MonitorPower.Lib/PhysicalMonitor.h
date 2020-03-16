#pragma once

using namespace System;

namespace RM
{
	namespace Win
	{
		namespace MonitorPower
		{
			namespace Lib
			{
				public enum class MonitorPowerState : UInt32
				{
					Unknown = 0x00,
					On = 0x01,
					Standby = 0x02,
					Suspend = 0x03,
					Off = 0x04,
				};

				public ref class PhysicalMonitor sealed
				{
					private:
						IntPtr _handle;
						String^ _description;

						PhysicalMonitor(const IntPtr handle, String^ description);
						inline void Destroy();

					protected:
						!PhysicalMonitor();

					public:
						property String^ Description
						{
							String^ get()
							{
								return _description;
							}
						}

						void SetPowerState(MonitorPowerState state);
						static array<PhysicalMonitor^>^ GetPhysicalMonitors();
						~PhysicalMonitor();
				};
			}
		}
	}
}
