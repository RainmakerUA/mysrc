namespace RM.BinPatcher.Model
{
	public class PatchEntry
	{
		public enum MatchBy
		{
			Address = 0,
			FirstMatch,
			SingleMatch,
			EveryMatch
		}

		internal PatchEntry(long? address, MatchBy match, Pattern oldData, Pattern newData)
		{
			Address = address;
			Match = match;
			OldData = oldData;
			NewData = newData;
		}

		public long? Address { get; }

		public MatchBy Match { get; }

		public Pattern OldData { get; }

		public Pattern NewData { get; }
	}
}
