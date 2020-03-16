#pragma once

namespace dxva2
{
	bool get_physical_monitors(std::vector<PHYSICAL_MONITOR>& vector);

	bool set_monitor_vcp_value(HANDLE monitor, BYTE code, DWORD new_value);

	bool destroy_monitor(HANDLE monitor);
}
