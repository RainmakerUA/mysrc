using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.InputRobot.Native;

namespace RM.InputRobot.Modules
{
	public sealed class Input
	{

		private static readonly INPUT[] _testInputsI = new[]
													/*{
														// H
														INPUT.From(new KEYBDINPUT {wVk = VK.SHIFT}),
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_H}),
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_H, dwFlags = KEYEVENTF.KEYUP}),
														INPUT.From(new KEYBDINPUT {wVk = VK.SHIFT, dwFlags = KEYEVENTF.KEYUP}),
														// e
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_E}),
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_E, dwFlags = KEYEVENTF.KEYUP}),
														// l
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_L}),
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_L, dwFlags = KEYEVENTF.KEYUP}),
														// l
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_L}),
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_L, dwFlags = KEYEVENTF.KEYUP}),
														// o
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_O}),
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_O, dwFlags = KEYEVENTF.KEYUP}),
														// !
														INPUT.From(new KEYBDINPUT {wVk = VK.SHIFT}),
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_1}),
														INPUT.From(new KEYBDINPUT {wVk = VK.VK_1, dwFlags = KEYEVENTF.KEYUP}),
														INPUT.From(new KEYBDINPUT {wVk = VK.SHIFT, dwFlags = KEYEVENTF.KEYUP}),
													};*/
														{
															INPUT.From(KEYBDINPUT.Char('H')),
															INPUT.From(KEYBDINPUT.Char('e')),
															INPUT.From(KEYBDINPUT.Char('l')),
															INPUT.From(KEYBDINPUT.Char('l')),
															INPUT.From(KEYBDINPUT.Char('o')),
															INPUT.From(KEYBDINPUT.Char('!')),
														};

		public static void ActivateProcessWindow(string processName)
		{
			var process = Process.GetProcessesByName(processName).FirstOrDefault();
			if (process != null)
			{
				var handle = process.MainWindowHandle;
				if (handle != IntPtr.Zero)
				{
					Win32.ShowWindow(handle, SW.SHOWNORMAL);
					Win32.SetForegroundWindow(handle);
				}
			}
		}

		public void TestInput()
		{
			ActivateProcessWindow("notepad");
			
			System.Threading.Thread.Sleep(200);

			Win32.SendInput(_testInputsI);

			System.Threading.Thread.Sleep(500);

			Win32.SendInput(new[] {INPUT.From(KEYBDINPUT.Char('\n')),});
			
			System.Threading.Thread.Sleep(200);

			Win32.SendInput(_testInputsI);
		}
	}
}
