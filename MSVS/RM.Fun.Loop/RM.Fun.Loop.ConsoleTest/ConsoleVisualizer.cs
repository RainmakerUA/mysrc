using System;
using RM.Fun.Loop.Lib;

namespace RM.Fun.Loop.ConsoleTest
{
	internal class ConsoleVisualizer : IFieldVisualizer
	{
		private static readonly char[] _elementChars =
														{
															'\u0020',
															'^',
															'>',
															'\u2514',
															'v',
															'\u2502',
															'\u250C',
															'\u251C',
															'<',
															'\u2518',
															'\u2500',
															'\u2534',
															'\u2510',
															'\u2524',
															'\u252C',
															'\u253C'
														};

		private readonly (int x, int y) _fieldStart;

		public ConsoleVisualizer(int startX, int startY)
		{
			_fieldStart = (startX, startY);
		}

		public void VisualizeCell(Field field, int horizontalIndex, int verticalIndex, bool applyRotation)
		{
			var cell = field[horizontalIndex, verticalIndex];
			var nodes = cell.Value;

			if (applyRotation)
			{
				nodes = RotateElement(nodes, cell.Rotation);
			}

			var (oldLeft, oldTop) = (Console.CursorLeft, Console.CursorTop);

			Console.SetCursorPosition(_fieldStart.x + horizontalIndex, _fieldStart.y + verticalIndex);
			Console.Write(_elementChars[nodes]);
			Console.SetCursorPosition(oldLeft, oldTop);

			byte RotateElement(byte nodesByte, byte rotation) => (byte)(((nodesByte << rotation) | (nodesByte >> (Field.MaxNodes - rotation))) & (1 << Field.MaxNodes));
		}
	}
}
