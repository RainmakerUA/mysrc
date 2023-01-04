using System.Collections.Generic;
using RM.BinPatcher.Parsers;

namespace RM.BinPatcher.Model
{
	public class Patch
	{
		internal Patch(IReadOnlyList<PatchEntry> entries, string? title = null, string? author = null, string? url = null)
		{
			Entries = entries;
			Title = title;
			Author = author;
			Url = url;
		}

		public string? Title { get; }

		public string? Author { get; }

		public string? Url { get; }

		public IReadOnlyList<PatchEntry> Entries { get; }

		public static Patch Parse(IReadOnlyList<string> patchLines)
		{
			return PatchParser.Parse(patchLines);
		}
	}
}
