using System;
using System.Collections;
using System.Collections.Generic;

namespace RM.CSharpTest.Sharp8
{
	public interface ITest0<T>
	{
		IList<T>? UnderList { get; }

		T[] ToArray()
		{
			if (UnderList == null)
			{
				return new T[0];
			}

			var result = new T[UnderList.Count];
			UnderList.CopyTo(result, 0);
			return result;
		}
	}

	public interface ITest1<T> : IList<T>
	{
		protected static List<int> _counts = new List<int>();

		int Length { get; }
	}

	public interface ITest1
	{
		private class Test1<T> : ITest1<T>, ITest0<T>
		{
			private readonly List<T> _list;

			public Test1(IEnumerable<T> elements)
			{
				_list = new List<T>(elements);
			}

			public T this[Index index]
			{
				get => _list[index.GetOffset(_list.Count)];
				set => _list[index.GetOffset(_list.Count)] = value;
			}

			public T[] this[Range range]
			{
				get
				{
					var (offset, length) = range.GetOffsetAndLength(_list.Count);
					return _list.GetRange(offset, length).ToArray();
				}
			}

			public int Length => _list.Count;

			public IList<T>? UnderList => _list;

			public T[] ToArray() => _list.ToArray();

			int ICollection<T>.Count => _list.Count;

			bool ICollection<T>.IsReadOnly => (_list as ICollection<T>).IsReadOnly;

			T IList<T>.this[int index]
			{
				get => _list[index];
				set => _list[index] = value;
			}

			int IList<T>.IndexOf(T el) => _list.IndexOf(el);

			void IList<T>.RemoveAt(int index) => _list.RemoveAt(index);

			void IList<T>.Insert(int index, T el) => _list.Insert(index, el);

			void ICollection<T>.Add(T el) => _list.Add(el);

			void ICollection<T>.Clear() => _list.Clear();

			bool ICollection<T>.Contains(T el) => _list.Contains(el);

			void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

			bool ICollection<T>.Remove(T item) => _list.Remove(item);

			IEnumerator<T> IEnumerable<T>.GetEnumerator() => _list.GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
		}

		public static ITest1<T> Create<T>(IEnumerable<T> elements)
		{
			return new Test1<T>(elements);
		}
	}
}
