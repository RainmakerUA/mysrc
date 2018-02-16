using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.CSharpTest
{
	public class ListTester
	{
		private class Item
		{
			public int Value { get; set; }
			public Item Next { get; set; }
		}

		private static Item ReadList(string input)
		{
			Item head = null;
			Item tail = null;
			if (!String.IsNullOrEmpty(input))
			{
				foreach (var el in input.Split(new[] { '\u0020' }, StringSplitOptions.RemoveEmptyEntries))
				{
					int value;
					if (!Int32.TryParse(el, out value))
					{
						value = -19;
					}
					var item = new Item { Value = value };
					if (tail == null)
					{
						tail = item;
						head = tail;
					}
					else
					{
						tail.Next = item;
						tail = item;
					}
				}
			}
			return head;
		}

		private static void PrintList(string title, Item head)
		{
			title = title ?? "List";
			Console.Write($"{title}:");
			do
			{
				Console.Write(" ");
				Console.Write(head.Value);
				head = head.Next;
			} while (head != null);
			Console.WriteLine();
		}

		private static Item ReverseList(Item head)
		{
			Item newHead = null;
			while (head != null)
			{
				var temp = head;
				head = head.Next;

				temp.Next = newHead;
				newHead = temp;
			}

			return newHead;
		}

		public void Run()
		{
			Console.Write("Gimme numbers: ");
			var head = ReadList(Console.ReadLine());
			PrintList("Original", head);
			var revHead = ReverseList(head);
			PrintList("Reversed", revHead);
		}
	}
}
