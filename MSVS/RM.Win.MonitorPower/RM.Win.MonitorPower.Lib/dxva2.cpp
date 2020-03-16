#include "pch.h"
#include "dxva2.h"

#pragma comment(lib, "user32.lib")
#pragma comment(lib, "dxva2.lib")

namespace dxva2
{
	#pragma unmanaged

	BOOL CALLBACK monitor_enum_proc(HMONITOR, HDC, LPRECT, LPARAM);

	bool get_physical_monitors(std::vector<PHYSICAL_MONITOR>& vector)
	{
		vector.clear();		
		return EnumDisplayMonitors(nullptr, nullptr, &monitor_enum_proc, reinterpret_cast<LPARAM>(&vector));
	}

	bool set_monitor_vcp_value(HANDLE monitor, BYTE code, DWORD new_value)
	{
		return SetVCPFeature(monitor, code, new_value) != 0;
	}

	bool destroy_monitor(HANDLE monitor)
	{
		return DestroyPhysicalMonitor(monitor);
	}

	BOOL CALLBACK monitor_enum_proc(HMONITOR monitor, HDC dc, LPRECT rect, LPARAM lparam)
	{
		auto vec = reinterpret_cast<std::vector<PHYSICAL_MONITOR>*>(lparam);
		DWORD num_monitors;

		if (GetNumberOfPhysicalMonitorsFromHMONITOR(monitor, &num_monitors) && num_monitors > 0)
		{
			std::unique_ptr<PHYSICAL_MONITOR[]> phys_monitors(new PHYSICAL_MONITOR[num_monitors]);

			if (GetPhysicalMonitorsFromHMONITOR(monitor, num_monitors, phys_monitors.get()))
			{
				for (size_t i = 0; i < num_monitors; i++)
				{
					vec->push_back(phys_monitors[i]);
				}
			}
		}

		return TRUE;
	}

	#pragma managed
}
