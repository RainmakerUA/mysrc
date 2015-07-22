using System;
using System.Linq;
using System.Threading;

namespace RM.CSharpTest
{
	internal class Program
    {
        private static void Main(string[] args)
        {
			var tester = new Tester("Mr. Tester", "mrtester@example.com");
			tester.TestingCompleted += (sender, message) => { Console.WriteLine(message); };

			tester.AddTests(Enumerable.Range(0, 100).Select(i => DateTime.Today.AddDays(i).AddHours(9).ToString("s")));
			tester.Start();

			Thread.Sleep(29000);

			tester.Stop();
			Console.WriteLine($"{tester.Name} tested {tester.TestedCount} so far");

			Console.WriteLine("Testing goes on. Press ENTER when your want");
			tester.Start();

			Console.ReadLine();
			tester.Stop();
        }
    }
}
