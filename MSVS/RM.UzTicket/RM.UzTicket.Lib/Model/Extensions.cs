using System.Collections.Generic;
using System.Json;

namespace RM.UzTicket.Lib.Model
{
	public static class Extensions
	{
		public static T Get<T>(this JsonObject obj, string key)
		{
			return obj[key].ReadAs<T>();
		}

		public static T GetValueOrDefault<T>(this JsonObject jobj, string key, T defaultValue = default)
		{
			return jobj?[key] != null && jobj[key].TryReadAs(out T result) ? result : defaultValue;
		}

		public static IEnumerable<Seat> GetSeats(this Coach coach, IReadOnlyDictionary<string, int[]> seatNumbers)
		{
			foreach (var charline in seatNumbers.Keys)
			{
				foreach (var seatNum in seatNumbers[charline])
				{
					yield return Seat.Create(charline, seatNum, coach.Prices.TryGetValue(charline, out var price) ? price : new decimal?());
				}
			}
		}
	}
}
