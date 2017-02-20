using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RM.WikiLinks.Providers
{
	internal sealed class WikiPageLinkProvider
	{
		private const string _wikiSearchUrlFormat = "https://{0}.wikipedia.org/w/api.php?action=opensearch&format=json&namespace=0&suggest=HTTP/1.1&limit={1}&search={2}";
		private const int _defaultSearchLimit = 10;

		private const string _wikiUrlFormat = "https://{0}.wikipedia.org{1}";
		private const string _wiki = "/wiki/";
		private const string _stop = "###stop###";
		private const int _linkLimit = 1000;

		private readonly string _langCode;
		private readonly object _mode;
		private readonly HtmlWeb _htmlWeb;

		public WikiPageLinkProvider()
		{
			_langCode = "ru";
			_mode = null;
			_htmlWeb = new HtmlWeb { AutoDetectEncoding = true, UsingCache = false };
		}

		public async Task<string[]> GetWikiSearch(string query, int? limit = null)
		{
			var uri = String.Format(_wikiSearchUrlFormat, _langCode, limit.GetValueOrDefault(_defaultSearchLimit), Uri.EscapeDataString(query));
			var req = WebRequest.Create(uri) as HttpWebRequest;
			req.Method = WebRequestMethods.Http.Get;
			req.Accept = "applcation/json";

			var deser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(object[]));
			var resp = await req.GetResponseAsync();

			using (var stream = resp.GetResponseStream())
			{
				var array = deser.ReadObject(stream) as object[];
				return Array.ConvertAll(array[1] as object[], o => o as string);
			}
		}

		public string[] GetWikiLinkPath(string beginTerm, string endTerm)
		{
			var begin = EncodeWikiTerm(beginTerm);
			var end = EncodeWikiTerm(endTerm);
			var pageLinks = new Dictionary<string, string> { { begin, _stop } };
			var linkQueue = new Queue<string>(new[] { begin });

			for (var i = 0; i < _linkLimit; i++)
			{
				var currentPage = linkQueue.Dequeue();

				foreach (var link in FindLinks(currentPage))
				{
					if (!pageLinks.ContainsKey(link))
					{
						pageLinks.Add(link, currentPage);

						if (link == end)
						{
							return MakeChain(pageLinks, link);
						}

						linkQueue.Enqueue(link);
						RaiseEvent(LinkProcessed, DecodeWikiTerm(currentPage), DecodeWikiTerm(link));
					}
				}

				RaiseEvent(PageProcessed, DecodeWikiTerm(currentPage), null);
			}

			return null;
		}

		#region Events

		public event EventHandler<WikiPageLinkEventArgs> PageProcessed;

		public event EventHandler<WikiPageLinkEventArgs> LinkProcessed;

		#endregion

		#region Privat Methods

		private IEnumerable<string> FindLinks(string wikiTerm)
		{
			var htmlDoc = _htmlWeb.Load(String.Format(_wikiUrlFormat, _langCode, wikiTerm));
			var nodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");
			return from a in nodes
				   let href = a.Attributes["href"].Value
				   where href.StartsWith(_wiki, StringComparison.OrdinalIgnoreCase) && !href.Contains(":") && !href.Contains("#")
				   select href;
		}

		private static string EncodeWikiTerm(string term)
		{
			return _wiki + Uri.EscapeDataString(term.Replace('\u0020', '_'));
		}

		private static string DecodeWikiTerm(string term)
		{
			return Uri.UnescapeDataString(term.Substring(_wiki.Length)).Replace('_', '\u0020');
		}

		private static string[] MakeChain(IDictionary<string, string> links, string endLink)
		{
			var list = new List<string> { DecodeWikiTerm(endLink) };
			var parent = links[endLink];

			while (parent != _stop)
			{
				list.Add(DecodeWikiTerm(parent));
				parent = links[parent];
			}

			list.Reverse();
			return list.ToArray();
		}

		private void RaiseEvent(EventHandler<WikiPageLinkEventArgs> handler, string pageName, string linkName)
		{
			if (handler != null)
			{
				handler.Invoke(this, new WikiPageLinkEventArgs(pageName, linkName));
			}
		}

		#endregion
	}

	internal sealed class WikiPageLinkEventArgs : EventArgs
	{
		private readonly string _pageName;
		private readonly string _linkName;

		public WikiPageLinkEventArgs(string pageName, string linkName)
		{
			_pageName = pageName;
			_linkName = linkName;
		}

		public string PageName
		{
			get { return _pageName; }
		}

		public string LinkName
		{
			get { return _linkName; }
		}
	}
}
