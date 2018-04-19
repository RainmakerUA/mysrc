using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using RM.UzTicket.Lib.Model;

namespace RM.UzTicket.Lib.Test
{
	internal sealed class MockService : IUzService
	{
		private readonly string _sessionID = $"Mock-SID: {Guid.NewGuid():D}";

		public string GetSessionId() => _sessionID;

		public void Dispose()
		{
			// Do nothing
		}

		public Task<Train> FetchTrainAsync(DateTime date, Station source, Station destination, string trainNumber)
		{
			return Task.FromResult(Train.Create(
											GetTrainNumber(), source, destination,
											date.Date.AddHours(7).AddMinutes(15),
											date.Date.AddHours(21).AddMinutes(53),
											new CoachType[0]
										));
		}

		public Task<Coach[]> ListCoachesAsync(Train train, CoachType coachType)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyDictionary<string, int[]>> ListSeatsAsync(Train train, Coach coach)
		{
			throw new NotImplementedException();
		}

		public Task<JsonValue> BookSeatAsync(Train train, Coach coach, Seat seat, string firstName, string lastName, bool? bedding = null)
		{
			throw new NotImplementedException();
		}

		private static string GetTrainNumber()
		{
			const char a = 'А';
			const char ya = 'Я';

			var rnd = new Random();

			return $"{rnd.Next(5, 200):D3}{(char)rnd.Next(a, ya + 1)}";
		}
	}
}
