using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace RM.InputRobot.Native
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct INPUT
	{
		private enum InputType : uint
		{
			MOUSE = 0,
			KEYBOARD = 1,
			HARDWARE = 2
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct InputUnion
		{
			[FieldOffset(0)]
			public MOUSEINPUT mi;

			[FieldOffset(0)]
			public KEYBDINPUT ki;

			[FieldOffset(0)]
			public HARDWAREINPUT hi;
		}

		private readonly InputType _type;
		private readonly InputUnion _u;

		private INPUT(InputType type, InputUnion u)
		{
			_type = type;
			_u = u;
		}

		public MOUSEINPUT mi => _type == InputType.MOUSE ? _u.mi : default(MOUSEINPUT);

		public KEYBDINPUT ki => _type == InputType.KEYBOARD ? _u.ki : default(KEYBDINPUT);

		public HARDWAREINPUT hi => _type == InputType.HARDWARE ? _u.hi : default(HARDWAREINPUT);

		public static int Size => Marshal.SizeOf<INPUT>();

		public static INPUT From(MOUSEINPUT mi)
		{
			return new INPUT(InputType.MOUSE, new InputUnion { mi = mi });
		}

		public static INPUT From(KEYBDINPUT ki)
		{
			return new INPUT(InputType.KEYBOARD, new InputUnion { ki = ki });
		}

		public static INPUT From(HARDWAREINPUT hi)
		{
			return new INPUT(InputType.HARDWARE, new InputUnion { hi = hi });
		}
	}
}