using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.UzTicket.Lib;
using RM.UzTicket.Lib.Model;
using Con = System.Console;

namespace RM.UzTicket.Console.Tests
{
	internal static class UzClientTest
	{
		public static async Task Run()
		{
			Station startStation, endStation;
			string stationName;
			var client = new UzClient();

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

			var trains = await client.ListTrainsAsync(DateTime.Today, startStation, endStation);

			if (trains != null)
			{
				Con.WriteLine("Trains found:");

				foreach (var train in trains)
				{
					Con.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
					Con.WriteLine(train.GetInfo());
				} 
			}
			else
			{
				Con.WriteLine("Trains not found");
			}

			var sessionId = client.GetSessionId();

			Con.WriteLine(sessionId);
		}
	}
}
