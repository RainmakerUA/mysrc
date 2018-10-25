using System;
using System.IO;
using System.Threading.Tasks;
using RM.Lib.Utility;
using RM.Lib.UzTicket.Model;

namespace RM.Lib.UzTicket
{
	public sealed class UzClient
	{
		private readonly string _baseUrl;
		private readonly string _sessionIdKey;
		private readonly UzScanner _scanner;

		public UzClient(string baseUrl, string sessionIdKey)
		{
			_baseUrl = baseUrl;
			_sessionIdKey = sessionIdKey;
			_scanner = new UzScanner(CreateService, Logger);
		}

		private static ILog Logger => LogFactory.GetLog();

		public event EventHandler<ScanEventArgs> ScanEvent
		{
			add => _scanner.ScanEvent += value;
			remove => _scanner.ScanEvent -= value;
		}

		public async Task<string[]> GetStationsAsync(string name)
		{
			using (var svc = CreateService())
			{
				return Array.ConvertAll(await svc.SearchStationAsync(name), st => st.ToString());
			}
		}

		/*
	    public Task<Station> GetFirstStationAsync(string name)
	    {
		    return _service.FetchFirstStationAsync(name);
	    }

	    public Task<Train[]> GetTrainsAsync(DateTime date, Station source, Station destination)
	    {
		    return _service.ListTrainsAsync(date, source, destination);
	    }
		*/

		public async Task LoadScans(params string[] scanLines)
		{
			using (var service = CreateService())
			{
				foreach (var line in scanLines)
				{
					var scanParts = line.Split('|');

					var callback = scanParts[0];
					var stFrom = await service.FetchFirstStationAsync(scanParts[1]);
					var stTo = await service.FetchFirstStationAsync(scanParts[2]);
					var date = DateTime.ParseExact(scanParts[3], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
					var train = scanParts[4];
					var coach = scanParts[5];
					var firstName = scanParts[6];
					var lastName = scanParts[7];

					_scanner.AddItem(new ScanItem(
										line, callback,
										firstName, lastName,
										date, stFrom, stTo,
										train, coach
									));
				}
			}
		}

		public void StartScan()
		{
			_scanner.Start();
		}

		public void StopScan()
		{
			_scanner.Stop();
		}

		/*
		public static void Initialize(IDependencyResolver resolver)
		{
			var uzClient = (UzClient)resolver.Get<IUzClient>();
			var teleClient = resolver.Get<ITelegramBot>();
			var host = resolver.Get<IApplicationHost>();

			var scanner = uzClient._scanner;

			scanner.ScanEvent += async (o, e) => await teleClient.SendMasterMessage($"<b>Scan successful!</b>\nTicket reserved for '{e.CallbackID}':\n{e.Message}");
			host.Started += (o, e) => scanner.Start();
			host.Stopping += (o, e) => scanner.Stop();

			uzClient.LoadScans().GetAwaiter().GetResult();
		}
		*/
		private UzService CreateService()
		{
			return new UzService(_baseUrl, _sessionIdKey, Logger);
		}
	}
}
