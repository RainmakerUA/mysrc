using System;

namespace RM.Fun.Loop.Lib
{
	public class Field
	{
		private class FieldCell : ICell
		{
			private readonly Field _field;

			private readonly int _hIndex;

			private readonly int _vIndex;

			public FieldCell(Field field, int hIndex, int vIndex)
			{
				_field = field;
				_hIndex = hIndex;
				_vIndex = vIndex;
			}

			//public FieldSides Sides => (FieldSides)Value;

			public byte Rotation => _field._rotation[_hIndex, _vIndex];

			public byte Value => _field._cells[_hIndex, _vIndex];
		}

		public const int MaxNodes = 4;

		private readonly int _width;
		private readonly int _height;

		private readonly byte[,] _cells;
		private readonly byte[,] _rotation;

		private readonly IFieldInitializer _initializer;

		internal Field(int width, int height, IFieldInitializer initializer)
		{
			_width = width > 0 ? width : throw new ArgumentOutOfRangeException(nameof(width));
			_height = height > 0 ? height : throw new ArgumentOutOfRangeException(nameof(height));
			_initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));

			_cells = new byte[width, height];
			_rotation = new byte[width, height];
		}

		public int Width => _width;

		public int Height => _height;

		public ICell this[int horizontalIndex, int verticalIndex] => new FieldCell(this, horizontalIndex, verticalIndex);

		public void Initialize(bool rotate = false)
		{
			var rnd = rotate ? new Random() : null;

			for (int horizontalIndex = 0; horizontalIndex < _width; horizontalIndex++)
			{
				for (int verticalIndex = 0; verticalIndex < _height; verticalIndex++)
				{
					var cell = _initializer.InitializeCell(horizontalIndex, verticalIndex, this);
					_cells[horizontalIndex, verticalIndex] = (byte)cell;

					if (rotate)
					{
						_rotation[horizontalIndex, verticalIndex] = (byte)rnd.Next(MaxNodes);
					}
				}
			}
		}

		public (FieldSides sides, byte rotation) GetCell(int horizontalIndex, int verticalIndex)
		{
			if (horizontalIndex < 0 || horizontalIndex >= _width)
			{
				throw new ArgumentOutOfRangeException(nameof(horizontalIndex));
			}

			if (verticalIndex < 0 || verticalIndex >= _height)
			{
				throw new ArgumentOutOfRangeException(nameof(verticalIndex));
			}

			return ((FieldSides)_cells[horizontalIndex, verticalIndex], _rotation[horizontalIndex, verticalIndex]);
		}

		public void Rotate(int horizontalIndex, int verticalIndex, bool reset = false)
		{
			if (reset)
			{
				_rotation[horizontalIndex, verticalIndex] = 0;
			}
			else
			{
				var rot = _rotation[horizontalIndex, verticalIndex] + 1;
				_rotation[horizontalIndex, verticalIndex] = (byte)(rot < MaxNodes ? rot : 0);
			}
		}
	}
}
