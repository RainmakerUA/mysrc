﻿
namespace RM.Shooter.Native
{
	internal enum HSHELL
	{
		WINDOWCREATED = 1,
		WINDOWDESTROYED = 2,
		ACTIVATESHELLWINDOW = 3,
		WINDOWACTIVATED = 4,
		GETMINRECT = 5,
		REDRAW = 6,
		TASKMAN = 7,
		LANGUAGE = 8,
		SYSMENU = 9,
		ENDTASK = 10,
		ACCESSIBILITYSTATE = 11,
		APPCOMMAND = 12,
		WINDOWREPLACED = 13,
		WINDOWREPLACING = 14,

		HIGHBIT = 0x8000,
		FLASH = REDRAW | HIGHBIT,
		RUDEAPPACTIVATED = WINDOWACTIVATED | HIGHBIT
	}
}
