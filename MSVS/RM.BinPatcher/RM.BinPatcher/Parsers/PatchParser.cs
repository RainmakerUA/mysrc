using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RM.BinPatcher.Exceptions;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Parsers
{
	internal static class PatchParser
	{
		private enum LineKind
		{
			Empty = 0,
			Address,
			AddressWithPattern,
			Pattern,
			Offset
		}

		private class Line
		{
			public LineKind Kind { get; set; }

			public string Comment { get; set; }

			public string Address { get; set; }

			public string Pattern { get; set; }
		}

		private const char _addressSeparator = ':';

		private const string _titlePrefix = "patch:";

		private const string _authorPrefix = "author:";

		private const string _urlPrefix = "url:";

		private const string _hexPrefix = "0x";

		private static readonly int _hexPrefixLength = _hexPrefix.Length;

		private static readonly char[] _commentChars = { ';', '#' };

		private static readonly char[] _offsetPrefixChars = { '+', '-' };

		private static readonly IDictionary<string, PatchEntry.MatchBy> _matchBys = new Dictionary<string, PatchEntry.MatchBy>
																						{
																							["?1"] = PatchEntry.MatchBy.FirstMatch,
																							["?!"] = PatchEntry.MatchBy.SingleMatch,
																							["?*"] = PatchEntry.MatchBy.EveryMatch
																						};

		public static Patch Parse(IReadOnlyList<string> patchLines)
		{
			if (patchLines == null)
			{
				throw new ArgumentNullException(nameof(patchLines));
			}

			var linesCount = patchLines.Count;
			var entries = new List<PatchEntry>();
			var inHeader = true;
			var offset = 0L;
			string title = null, author = null, url = null;
			Line lineM1 = null, lineM2 = null;

			for (int i = 0; i < linesCount; i++)
			{
				var patchLine = patchLines[i];

				try
				{
					var line = ParseLine(patchLine);

					switch (line.Kind)
					{
						case LineKind.Empty:
							if (inHeader && !String.IsNullOrEmpty(line.Comment))
							{
								ParseHeader(line.Comment, ref title, ref author, ref url);
							}
							break;
						case LineKind.Address:
						case LineKind.AddressWithPattern:
							if (lineM2 != null || lineM1 != null)
							{
								throw new Exception("Unexpected address!");
							}
							inHeader = false;
							lineM1 = line;
							break;
						case LineKind.Offset:
							if (lineM2 != null || lineM1 != null)
							{
								throw new Exception("Unexpected offset!");
							}
							inHeader = false;
							offset = ParseOffset(line.Address);
							break;
						case LineKind.Pattern:
							inHeader = false;
							if (lineM1 == null)
							{
								throw new InvalidOperationException("Found pattern without address!");
							}
							if (lineM2 == null && lineM1.Kind == LineKind.Address)
							{
								lineM2 = lineM1;
								lineM1 = line;
							}
							else
							{
								var entry = ParseEntry(lineM2, lineM1, line, offset);
								entries.Add(entry);
								lineM2 = null;
								lineM1 = null;
							}
							break;
						default:
							throw new Exception("Cannot parse line!");
					}
				}
				catch (Exception e)
				{
					throw new PatchParseException(patchLine, i + 1, e);
				}
			}

			// TODO: validation for empty patch?
			// TODO: validate patch contents: overlapping addresses etc. (?or in Patcher.Validate)

			return new Patch(entries, title, author, url);
		}

		private static Line ParseLine(string line)
		{
			var result = new Line();
			var commentPos = _commentChars.Select(
												c =>
												{
													var idx = line.IndexOf(c);
													return idx >= 0 ? idx : new int?();
												}
											).Min();

			if (commentPos.HasValue)
			{
				var comment = line.Substring(commentPos.Value + 1).Trim();

				if (!String.IsNullOrEmpty(comment))
				{
					result.Comment = comment;
				}

				line = line.Substring(0, commentPos.Value);
			}

			line = line.Trim();

			int index;

			if (String.IsNullOrEmpty(line))
			{
				result.Kind = LineKind.Empty;
			}
			else if ((index = line.IndexOf(_addressSeparator)) >= 0)
			{
				var address = line.Substring(0, index).Trim();
				var afterAddress = line.Substring(index + 1).Trim();

				result.Address = address;

				if (String.IsNullOrEmpty(afterAddress))
				{
					result.Kind = LineKind.Address;
				}
				else
				{
					result.Kind = LineKind.AddressWithPattern;
					result.Pattern = afterAddress;
				}
			}
			else if (Array.IndexOf(_offsetPrefixChars, line[0]) >= 0)
			{
				result.Kind = LineKind.Offset;
				result.Address = line;
			}
			else
			{
				result.Kind = LineKind.Pattern;
				result.Pattern = line;
			}

			return result;
		}

		private static void ParseHeader(string comment, ref string title, ref string author, ref string url)
		{
			if (String.IsNullOrEmpty(author)
				&& comment.StartsWith(_authorPrefix, StringComparison.OrdinalIgnoreCase))
			{
				author = comment.Substring(_authorPrefix.Length + 1).Trim();
			}

			if (String.IsNullOrEmpty(url)
				&& comment.StartsWith(_urlPrefix, StringComparison.OrdinalIgnoreCase))
			{
				url = comment.Substring(_urlPrefix.Length + 1).Trim();
			}

			if (String.IsNullOrEmpty(title))
			{
				if (comment.StartsWith(_titlePrefix, StringComparison.OrdinalIgnoreCase))
				{
					author = comment.Substring(_authorPrefix.Length + 1).Trim();
				}
				else if (String.IsNullOrEmpty(author) && String.IsNullOrEmpty(url))
				{
					title = comment.Trim();
				}
			}
		}

		private static long ParseOffset(string offset)
		{
			var negative = offset[0] == _offsetPrefixChars[1];
			var hex = offset.Length > _hexPrefixLength + 1
						&& offset.Substring(1, _hexPrefixLength).Equals(_hexPrefix, StringComparison.OrdinalIgnoreCase);

			var num = Int64.Parse(
								hex ? offset.Substring(_hexPrefixLength + 1) : offset,
								hex ? NumberStyles.HexNumber : NumberStyles.Integer
							);
			return hex && negative ? -num : num;
		}

		private static PatchEntry ParseEntry(Line lineM2, Line lineM1, Line line, long offset)
		{
			var isOldDataWithAddress = lineM2 == null;
			var addressData = ParseAddress(isOldDataWithAddress ? lineM1.Address : lineM2.Address);
			var oldData = PatternParser.ParseBytes(lineM1.Pattern);
			var newData = PatternParser.ParseBytes(line.Pattern);
			AdjustNewDataLength(oldData, ref newData);

			return new PatchEntry(addressData.Item1 + offset, addressData.Item2, new Pattern(oldData), new Pattern(newData));
		}

		private static Tuple<long?, PatchEntry.MatchBy> ParseAddress(string addr)
		{
			if (_matchBys.ContainsKey(addr))
			{
				return Tuple.Create(new long?(), _matchBys[addr]);
			}

			var hex = addr.Length > _hexPrefixLength + 1
						&& addr.Substring(0, _hexPrefixLength).Equals(_hexPrefix, StringComparison.OrdinalIgnoreCase);
			var address = Int64.Parse(
									hex ? addr.Substring(_hexPrefixLength) : addr,
									hex ? NumberStyles.HexNumber : NumberStyles.Integer
								);
			return Tuple.Create(new long?(address), PatchEntry.MatchBy.Address);
		}

		private static void AdjustNewDataLength<T>(T[] oldData, ref T[] newData) where T : struct
		{
			var oldDataLength = oldData.Length;
			var newDataLength = newData.Length;

			if (oldDataLength < newDataLength)
			{
				throw new Exception("Old data is shorter than new data!");
			}

			if (oldDataLength  > newDataLength)
			{
				// TODO: Maybe a warning?
				Array.Resize(ref newData, oldDataLength);
			}
		}
	}
}
