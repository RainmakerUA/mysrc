#include "pch.h"
#include "PhysicalMonitor.h"
#include "dxva2.h"

using namespace System::Collections::Generic;

namespace RM
{
	namespace Win
	{
		namespace MonitorPower
		{
			namespace Lib
			{
				PhysicalMonitor::PhysicalMonitor(const IntPtr handle, String^ description)
				{
					_handle = handle;
					_description = description;
				}

				void PhysicalMonitor::Destroy()
				{
					dxva2::destroy_monitor(_handle.ToPointer());
				}

				PhysicalMonitor::!PhysicalMonitor()
				{
					Destroy();
				}

				PhysicalMonitor::~PhysicalMonitor()
				{
					Destroy();
				}

				void PhysicalMonitor::SetPowerState(MonitorPowerState state)
				{
					constexpr BYTE MONITOR_POWER = 0xD6;
					if (!dxva2::set_monitor_vcp_value(_handle.ToPointer(), MONITOR_POWER, safe_cast<DWORD>(state)))
					{
						throw gcnew ComponentModel::Win32Exception(Runtime::InteropServices::Marshal::GetLastWin32Error());
					}
				}

				array<PhysicalMonitor^>^ PhysicalMonitor::GetPhysicalMonitors()
				{
					std::vector<PHYSICAL_MONITOR> monitors;

					if (!dxva2::get_physical_monitors(monitors))
					{
						throw gcnew ComponentModel::Win32Exception(Runtime::InteropServices::Marshal::GetLastWin32Error());
					}
					
					auto result = gcnew List<PhysicalMonitor^>();

					for (auto const& monitor: monitors)
					{
						result->Add(gcnew PhysicalMonitor(IntPtr(monitor.hPhysicalMonitor), gcnew String(monitor.szPhysicalMonitorDescription)));
					}

					return result->ToArray();
				}
			}
		}
	}
}
