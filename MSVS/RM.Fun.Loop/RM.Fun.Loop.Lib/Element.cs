using System;
using System.Runtime.InteropServices;

namespace RM.Fun.Loop.Lib
{
	/*[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Element
	{
		public const byte MaxNodes = 4;

		private readonly byte _count;

		private readonly byte _nodes;

		public Element(bool[] nodes)
		{
			ValidateNodes(nodes);
			(_count, _nodes) = ParseNodes(nodes);
		}

		public byte Nodes => _nodes;

		public byte NodeCount => _count;

		public bool[] NodesArray => ToNodes(_nodes);

		public bool this[byte i] => HasNode(_nodes, i);

		public override int GetHashCode()
		{
			return (_count << 8) | _nodes;
		}

		private static void ValidateNodes(bool[] nodes)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException(nameof(nodes));
			}

			var length = nodes.Length;

			if (length > MaxNodes)
			{
				throw new ArgumentOutOfRangeException(nameof(nodes), "Nodes count is out of range");
			}
		}

		private static (byte, byte) ParseNodes(bool[] nodes)
		{
			byte count = 0;
			byte nodeByte = 0;

			for (var i = 0; i < nodes.Length; i++)
			{
				if (nodes[i])
				{
					count++;
					nodeByte |= (byte)(1 << i);
				}
			}

			return (count, nodeByte);
		}

		private static bool[] ToNodes(byte nodeByte)
		{
			var result = new bool[MaxNodes];

			for (byte i = 0; i < MaxNodes; i++)
			{
				result[i] = HasNode(nodeByte, i);
			}

			return result;
		}

		private static bool HasNode(byte nodeByte, byte node) => (nodeByte & (byte)(1 << node)) != 0;
	}*/
}
