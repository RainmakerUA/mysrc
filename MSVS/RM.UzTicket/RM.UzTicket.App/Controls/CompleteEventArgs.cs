using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.UzTicket.App.Controls
{
	public sealed class CompleteAsyncEventArgs : EventArgs
	{
		public CompleteAsyncEventArgs(string request)
		{
			Request = request;
		}

		public string Request { get; }

		public Task<IReadOnlyList<string>> SuggestionsAsync { get; set; }
	}
}
