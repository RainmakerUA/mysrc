using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using RM.Fun.Loop.Lib;

namespace RM.Fun.Loop.ConsoleTest
{
	internal class Program
	{
		// TODO: Move to separate file (in Lib?) and add option for symmetric fill.
		private class Initializer : IFieldInitializer
		{
			private readonly Random _rnd = new Random();
			private readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
			private readonly byte[] _rngByte = new byte[1];

			public FieldSides InitializeCell(int horizontalIndex, int verticalIndex, Field field)
			{
				var nodes = FieldSides.None;
				//var rnd = new Random();

				nodes |= verticalIndex > 0 && field[horizontalIndex, verticalIndex - 1].HasSide(FieldSides.Bottom) ? FieldSides.Top : FieldSides.None;
				nodes |= horizontalIndex > 0 && field[horizontalIndex - 1, verticalIndex].HasSide(FieldSides.Right) ? FieldSides.Left : FieldSides.None;

				nodes |= horizontalIndex < field.Width - 1 && GetRandomBool(0.4) ? FieldSides.Right : FieldSides.None;
				nodes |= verticalIndex < field.Height - 1 && GetRandomBool(0.4) ? FieldSides.Bottom : FieldSides.None;

				return nodes;
			}

			private bool GetRandomBool(double margin = 0.5)
			{
				_rng.GetBytes(_rngByte);
				return _rngByte[0] < (byte)Math.Round((Byte.MaxValue + 1) * margin);
			}

			private bool GetRandomBoolOld(double margin = 0.5)
			{
				return _rnd.NextDouble() < margin;
			}
		}


		private static void Main(string[] args)
		{
			//var els = new[]
			//			{
			//				new Element(new[] {true}),
			//				new Element(new[] {false, true}),
			//				new Element(new[] {false, false, true}),
			//				new Element(new[] {false, false, false, true}),
			//				new Element(new[] {true, true}),
			//				new Element(new[] {false, true, false, true}),
			//				new Element(new[] {true, false, true, true}),
			//				new Element(new[] {true, true, true}),
			//				new Element(new[] {true, true, true, true})
			//			};

			////Console.OutputEncoding = Encoding.GetEncoding(1251);

			//foreach (var el in els)
			//{
			//	Console.WriteLine(ElementToString(el));
			//}

			Console.Clear();

			Game g;

			do
			{
				Console.Clear();
				g = new Game(30, 20, new Initializer(), new ConsoleVisualizer(5, 2));
				g.DrawField();
			} while (Console.ReadKey(true).Key == ConsoleKey.Enter);

			/*
			do
			{
				var (f, t, fs,ts) = GetRandomBools(1_000_000);

				Console.WriteLine("[F]:{0}", new String('=', (int)(80 * f / 1_000_000)));
				Console.WriteLine("[T]:{0}", new String('=', (int)(80 * t / 1_000_000)));
				Console.WriteLine("Max Fs in line: {0}", fs);
				Console.WriteLine("Max Ts in line: {0}", ts);
				Console.WriteLine();
			} while (Console.ReadKey(true).Key == ConsoleKey.Enter);
			*/

			/*
			var elChars = typeof(ConsoleVisualizer).GetField("_elementChars", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as char[];

			if (elChars != null)
			{
				for (int i = 0; i < elChars.Length; i++)
				{
					Console.WriteLine($"{i:D2}: {elChars[i]}");
				}
			}
			*/

			Console.Write("Press any key to exit: ");
			Console.ReadKey(true);
		}

		//private static string ElementToString(Element element)
		//{
		//	return String.Join("_", element.NodesArray.Select(n => n ? "+" : "\u0020"));
		//}

		private static (uint falses, uint trues, uint maxFStrife, uint maxTStrife) GetRandomBools(uint totalNum, double threshold = 0.5)
		{
			var rngCsp = new RNGCryptoServiceProvider();
			var rngByte = new byte[1];
			//var rnd = new Random();
			uint f = 0, t = 0;
			uint ff = 0, ffMax = 0;
			uint tt = 0, ttMax = 0;
			bool? prev = null;

			for (uint i = 0; i < totalNum; i++)
			{
				rngCsp.GetBytes(rngByte);

				if ((rngByte[0] & 1) == 1)
				//if (rnd.NextDouble() < threshold)
				{
					t++;

					if (prev.HasValue)
					{
						if (prev.Value)
						{
							tt++;
						}
						else
						{
							prev = true;

							if (ff > ffMax)
							{
								ffMax = ff;
							}

							ff = 0;
						}
					}
					else
					{
						tt = 1;
						prev = true;
					}
				}
				else
				{
					f++;

					if (prev.HasValue)
					{
						if (!prev.Value)
						{
							ff++;
						}
						else
						{
							prev = false;

							if (tt > ttMax)
							{
								ttMax = tt;
							}

							tt = 0;
						}
					}
					else
					{
						ff = 1;
						prev = false;
					}
				}
			}

			return (f, t, ffMax, ttMax);
		}
	}
}
