using System;
using System.Runtime.CompilerServices;

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
			Console.WriteLine("SizeOf(...) = {0}", Unsafe.SizeOf<ValueTuple<string>>());
			Console.WriteLine("Offset = {0}", Unsafe.ByteOffset(ref z2.Item4, ref z1.Item4));
			Console.WriteLine(Unsafe.AddByteOffset(ref z1.Item4, new IntPtr(8)));
		}
	}
}
