using System;
using System.Windows.Forms;

namespace RM.BossKey.Components
{
	internal struct HotkeyID : IEquatable<HotkeyID>
	{
		private readonly uint _flags;
		private readonly int _key;
		public readonly int ID;

		public static HotkeyID Empty = new HotkeyID();

		private HotkeyID(uint flags, int key)
		{
			_flags = flags;
			_key = key;
			ID = MakeID(flags, key);
		}

		public static HotkeyID Create(Modifiers flags, Keys key)
		{
			return new HotkeyID((uint)flags, (int)key);
		}

		public static bool operator ==(HotkeyID first, HotkeyID second)
		{
			return first.ID == second.ID;
		}

		public static bool operator !=(HotkeyID first, HotkeyID second)
		{
			return !(first == second);
		}

		public bool Equals(HotkeyID other)
		{
			return ID == other.ID;
		}

		public override bool Equals(object obj)
		{
			return obj is HotkeyID && Equals((HotkeyID)obj);
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		private static int MakeID(uint flags, int key)
		{
			return (key & 0xFFFF) << 16 | ((int)flags & 0xFFFF);
		}
	}
}
