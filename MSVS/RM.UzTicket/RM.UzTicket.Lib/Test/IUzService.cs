using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using RM.UzTicket.Lib.Model;

namespace RM.UzTicket.Lib.Test
{
	internal interface IUzService : IDisposable
	{
		string GetSessionId();

		
		Task<Train> FetchTrainAsync(DateTime date, Station source, Station destination, string trainNumber);

		Task<Coach[]> ListCoachesAsync(Train train, CoachType coachType);

		Task<IReadOnlyDictionary<string, int[]>> ListSeatsAsync(Train train, Coach coach);

		Task<JsonValue> BookSeatAsync(Train train, Coach coach, Seat seat, string firstName, string lastName, bool? bedding = null);
	}
}
