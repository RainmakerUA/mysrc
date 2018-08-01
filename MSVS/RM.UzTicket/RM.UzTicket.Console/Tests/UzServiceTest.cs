using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RM.UzTicket.Lib;
using RM.UzTicket.Lib.Exceptions;
using RM.UzTicket.Lib.Model;
using Con = System.Console;

namespace RM.UzTicket.Console.Tests
{
	internal static class UzServiceTest
	{
		public static async Task Run(bool routeTest = false)
		{
			Station startStation, endStation;
			Train train;
			CoachType coachType;
			Coach coach;
			string stationName, trainNum, coachLetter;
			string[] res;
			int seatNumber;

			var client = new UzService();

			try
			{
				do
				{
					Con.Write("Enter start station name: ");
					stationName = Con.ReadLine();
					startStation = await client.FetchFirstStationAsync(stationName);
					Con.WriteLine(startStation == null ? "Station not found. Try again." : $"Found: {startStation}");
				} while (startStation == null);

				do
				{
					Con.Write("Enter end station name: ");
					stationName = Con.ReadLine();
					endStation = await client.FetchFirstStationAsync(stationName);
					Con.WriteLine(endStation == null ? "Station not found. Try again." : $"Found: {endStation}");
				} while (endStation == null);

				Con.Write("Enter date (dd.mm.yyyy): ");
				var date = DateTime.ParseExact(Con.ReadLine(), "dd.MM.yyyy", null, DateTimeStyles.AssumeLocal);

				var trains = await client.ListTrainsAsync(date, startStation, endStation);

				if (trains != null)
				{
					Con.WriteLine("Trains found:");

					foreach (var tr in trains)
					{
						Con.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
						Con.WriteLine(tr.GetInfo());
					}
				}
				else
				{
					Con.WriteLine("Trains not found");
					return;
				}

				do
				{
					Con.Write("Enter train number and coach type: ");
					res = Con.ReadLine().Split(new[] {'\u0020', ',', ';'}, StringSplitOptions.RemoveEmptyEntries);
					trainNum = res[0];
					coachLetter = res[1];
					train = trains.FirstOrDefault(t => t.Number == trainNum);
					coachType = train?.CoachTypes?.FirstOrDefault(ct => ct.Letter == coachLetter);
				} while (train == null || coachType == null);

				if (routeTest)
				{
					var routes = await client.GetTrainRoutes(
												RouteData.Create(startStation, endStation, train.DepartureTime, train)
											);

					Con.WriteLine(routes[0].GetInfo());
					return;
				}

				var coaches = await client.ListCoachesAsync(train, coachType);

				if (coaches != null)
				{
					Con.WriteLine("Coaches:");

					foreach (var cch in coaches)
					{
						Con.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
						Con.WriteLine(cch.GetInfo());
					}
				}
				else
				{
					Con.WriteLine("Coaches not found!");
					return;
				}

				do
				{
					Con.Write("Enter coach number: ");
					var coachNum = Con.ReadLine();
					coach = coaches.FirstOrDefault(c => c.Number.ToString() == coachNum);
				} while (coach == null);

				var seats = await client.ListSeatsAsync(train, coach);
				
				Con.WriteLine("Seats: ");
				Con.WriteLine(String.Join(Environment.NewLine, seats.Select(kv => $"{kv.Key}: {String.Join("\u0020", kv.Value.Select(v => v.ToString("D2")))}")));

				do
				{
					Con.Write("Enter seat number: ");
					var seatNum = Con.ReadLine();
					seatNumber = Int32.Parse(seatNum);
				} while (seatNumber == 0);

				Seat seat = null;

				foreach (var charline in seats.Keys)
				{
					if ( Array.IndexOf(seats[charline], seatNumber) >= 0)
					{
						seat = Seat.Create(charline, seatNumber);
						break;
					}
				}

				if (seat == null)
				{
					throw new Exception("Seat is not found!");
				}

				Con.Write("Enter firstname and lastname: ");

				res = Con.ReadLine().Split(new[] {'\u0020', ',', ';'}, StringSplitOptions.RemoveEmptyEntries);

				Con.WriteLine("Booking selected seat...");

				var bl = await client.BookSeatAsync(train, coach, seat, res[0], res[1]);

				var sessionId = client.GetSessionId();
				Con.WriteLine($"In console: document.cookie='{sessionId}'; window.location.pathname='/cart'");
			}
			catch (ResponseException re)
			{
				Con.WriteLine($"Error: {re.Message}");
			}
			finally
			{
				client.Dispose();
			}
		}
	}
}
