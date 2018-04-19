
using System.Runtime.InteropServices;

namespace RM.Fun.Loop.Lib
{
	/*[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Cell
	{
		private readonly Element _element;
		private byte _rotation;

		internal Cell(Element element, byte rotation = 0)
		{
			_element = element;
			_rotation = (byte)(rotation % Element.MaxNodes);
		}

		public Element Element => _element;

		public byte Rotation
		{
			get => _rotation;
			internal set => _rotation = value;
		}

		public override int GetHashCode()
		{
			return _element.GetHashCode();
		}

		internal void Rotate(bool reset = false)
		{
			if (reset)
			{
				_rotation = 0;
			}
			else
			{
				_rotation += 1;

				if (_rotation >= Element.MaxNodes)
				{
					_rotation = 0;
				}
			}
		}
	}*/
}
