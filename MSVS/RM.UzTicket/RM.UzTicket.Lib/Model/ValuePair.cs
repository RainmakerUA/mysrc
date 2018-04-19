
namespace RM.UzTicket.Lib.Model
{
	public struct ValuePair<T> where T : struct
	{
		public readonly T First;

		public readonly T Second;

		public ValuePair(T first, T second)
		{
			First = first;
			Second = second;
		}
	}

	public static class ValuePair
	{
		public static ValuePair<T> From<T>(T first, T second) where T : struct
		{
			return new ValuePair<T>(first, second);
		}
	}
}
