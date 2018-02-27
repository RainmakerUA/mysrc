using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RM.CSharpTest
{
	internal static class TestUnsafe
	{
		public static void Run()
		{
			var z1 = (1, 2L, 'Z', "test");
			var zz = "new mark test";
			var z2 = (10, 20L, 'Z', "0test");


			Console.WriteLine("SizeOf(...) = {0}", Unsafe.SizeOf<ValueTuple<int, int, UIntPtr>>());
			Console.WriteLine("Offset = {0}", Unsafe.ByteOffset(ref z2, ref z1));
			Console.WriteLine(Unsafe.AddByteOffset(ref zz, new IntPtr(1)));
		}
	}
}
