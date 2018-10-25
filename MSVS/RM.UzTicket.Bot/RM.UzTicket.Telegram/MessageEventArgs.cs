using System;

namespace RM.UzTicket.Telegram
{
	public sealed class MessageEventArgs : BotEventArgs
	{
		public MessageEventArgs(long sender, string message) : base(sender)
		{
			Message = message;
		}

		public string Message { get; }
	}
}
