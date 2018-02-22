using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.UzTicket.Lib.Model;

namespace RM.UzTicket.Lib.Test
{
	internal interface IUzService : IDisposable
	{
		string GetSessionId();



		Task<Train> FetchTrainAsync(DateTime date, Station source, Station destination, string trainNumber);
	}
}
