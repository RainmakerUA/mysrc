using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Fun.Loop.Lib
{
	[Flags]
    public enum FieldSides : byte
    {
		None = 0,
		Top = 1,
		Right = 2,
		Bottom = 4,
		Left = 8
    }
}
