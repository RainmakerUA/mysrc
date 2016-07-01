using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.UzTicket.Lib.Utils
{
	internal static class Logger
	{
		public static void Debug(string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}
	}
}
