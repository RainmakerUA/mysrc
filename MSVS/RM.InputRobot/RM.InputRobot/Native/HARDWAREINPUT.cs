using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace RM.InputRobot.Native
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct HARDWAREINPUT
	{
		public uint uMsg;
		public ushort wParamL;
		public ushort wParamH;
	}
}