using System;

namespace RM.UzTicket.Telegram
{
	public enum Command
	{
		None = 0,
		Station,
		Train,

#if DEBUG
		Test = 999999
#endif
	}
}
