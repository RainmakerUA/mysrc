using System;
using System.Runtime.InteropServices;

namespace RM.CSharpTest
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			//var tester = new Tester("Mr. Tester", "mrtester@example.com");
			//tester.TestingCompleted += (sender, message) => { Console.WriteLine(message); };

			//tester.AddTests(Enumerable.Range(0, 100).Select(i => DateTime.Today.AddDays(i).AddHours(9).ToString("s")));
			//tester.Start();

			//Thread.Sleep(29000);

			//tester.Stop();
			//Console.WriteLine($"{tester.Name} tested {tester.TestedCount} so far");

			//Console.WriteLine("Testing goes on. Press ENTER when your want");
			//tester.Start();

			//Console.ReadLine();
			//tester.Stop();

			//Static.WriteLine("blah-blah!");
			//StaticBfi.WriteLine("blah-blah to you!");

			//Console.WriteLine("Static prop: {0}", Static.Stat.GetHashCode());
			//Console.WriteLine("StaticBfi prop: {0}", StaticBfi.Stat.GetHashCode());
			/*
			Console.WriteLine(Morse.ToCode("ПРИВЕТ, МАТРИКС42!"));

			var _ = new Morse();
			var ___ = new Morse(withSpace: true);

			Console.WriteLine(+- -+_+-+_+ +_+- -_+_-_+-+-+-___- -_+-_-_+-+_+ +_-+-_+ + + _+ + + +-_+ +- - -_- -+ +- -_);
			*/

			/*
			var lt = new ListTester();
			lt.Run();
			*/

			//Prynt("Test [Optional]: ");

			//TestUnsafe.Run();

			QuickTest();

			Console.WriteLine("Press ENTER to exit");
			Console.ReadLine();
		}

		private static void QuickTest()
		{
			var rnd = new Random();
			var srcArray = new byte[256];
			var srcArr2 = new[] { "test1", "test2", Guid.NewGuid().ToString("D") };

			rnd.NextBytes(srcArray);

			var targArray = srcArray.Clone() as byte[];
			var targArr2 = srcArr2.Clone() as string[];

			Console.WriteLine(String.Join(", ", targArray));
			Console.WriteLine(String.Join(", ", targArr2));

			Console.WriteLine($"1: {srcArray.GetHashCode()} :: {targArray.GetHashCode()}");
			Console.WriteLine($"2: {srcArr2.GetHashCode()} :: {targArr2.GetHashCode()}");
		}

		private static void Prynt(string title, [Optional] object value)
		{
			Console.Write(title);
			Console.WriteLine(value);
		}
	}
}
