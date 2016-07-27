﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RM.UzTicket.Lib.Exceptions;
using RM.UzTicket.Lib.Model;
using RM.UzTicket.Lib.Utils;

namespace RM.UzTicket.Lib
{
	internal sealed class UzScanner : IDisposable
	{
		private class ScanData
		{
			private const int _lockTimeout = 100;
			private readonly AutoResetEvent _lock;

			public ScanData(ScanItem item)
			{
				Item = item;
				Attempts = 0;

				_lock = new AutoResetEvent(true);
			}

			public ScanItem Item { get; }

			public int Attempts { get; private set; }

			public string Error { get; private set; }

			public AsyncLock GetLock()
			{
				return new AsyncLock(_lock, _lockTimeout);
			}

			public void IncAttempts()
			{
				Attempts++;
			}

			public void SetError(string error)
			{
				Error = error;
			}
		}

		private const int _defaultDelay = 60;

		private readonly Func<string, string, Task> _successCallbackAsync;
		private readonly int _delay;
		private readonly IDictionary<string, ScanData> _scanStates;
		private readonly UzClient _client;
		private readonly CancellationTokenSource _cancelTokenSource;
		
		private volatile bool _isRunning;
		private bool _isDisposed;

		public UzScanner(Func<string, string, Task> successCallbackAsync, int secondsDelay = _defaultDelay)
		{
			_successCallbackAsync = successCallbackAsync;
			_delay = secondsDelay;
			_scanStates = new ConcurrentDictionary<string, ScanData>();
			_client = new UzClient();
			_cancelTokenSource = new CancellationTokenSource();
		}

		#region Disposable
		
		~UzScanner()
		{
			Dispose(false);
		}

		private void Dispose(bool isDisposing)
		{
			if (!_isDisposed)
			{
				if (isDisposing)
				{
					// Free managed resources
					Reset();

					_client.Dispose();
					_cancelTokenSource.Dispose();
				}

				// Free unmanagement resources
				// ...

				_isDisposed = true;
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		#endregion

		public string AddItem(ScanItem item)
		{
			var scanId = Guid.NewGuid().ToString("N").ToUpperInvariant();

			_scanStates.Add(scanId, new ScanData(item));

			if (!_isRunning)
			{
				Start();
			}

			return scanId;
		}

		public Tuple<int, string> GetStatus(string scanId)
		{
			ScanData data;

			if (_scanStates.TryGetValue(scanId, out data))
			{
				return Tuple.Create(data.Attempts, data.Error);
			}

			throw new ScanNotFoundException(scanId);
		}

		public void Abort(string scanId)
		{
			if (!_scanStates.ContainsKey(scanId))
			{
				throw new ScanNotFoundException(scanId);
			}

			_scanStates.Remove(scanId);

			if (!_scanStates.Any())
			{
				Stop();
			}
		}

		public void Reset()
		{
			_isRunning = false;
			_cancelTokenSource.Cancel();
			_scanStates.Clear();
		}

		private void Start()
		{
			Task.Run(() => Run(), _cancelTokenSource.Token);
		}

		private void Stop()
		{
			_isRunning = false;
		}

		private async void Run()
		{
			//logInfo("Starting UzScanner");

			_isRunning = true;
			//TODO: stats?

			while (_isRunning)
			{
				foreach (var statePair in _scanStates)
				{
					await ScanAsync(statePair.Key, statePair.Value);
				}

				await Task.Delay(TimeSpan.FromSeconds(_delay));
			}
		}

		private void HandleError(string scanId, ScanData data, string error)
		{
			// TODO: Logging
			data.SetError(error);
		}

		private async Task ScanAsync(string scanId, ScanData data)
		{
			using (var lck = data.GetLock())
			{
				if (lck.IsCaptured)
				{
					data.IncAttempts();

					var item = data.Item;
					var train = await _client.FetchTrainAsync(item.Date, item.Source, item.Destination, item.TrainNumber);

					if (train != null)
					{
						CoachType[] coachTypes;

						if (!String.IsNullOrEmpty(item.CoachType))
						{
							var coachType = FindCoachType(train, item.CoachType);

							if (coachType == null)
							{
								HandleError(scanId, data, $"Coach type {item.CoachType} not found");
								return;
							}

							coachTypes = new[] { coachType };
						}
						else
						{
							coachTypes = train.CoachTypes;
						}

						var sessionId = await BookAsync(train, coachTypes, item.FirstName, item.LastName);

						if (!String.IsNullOrEmpty(sessionId))
						{
							await _successCallbackAsync(item.CallbackId, sessionId);
							Abort(scanId);
						}
						else
						{
							HandleError(scanId, data, "No vacant seats");
						}
					}
					else
					{
						HandleError(scanId, data, $"Train {item.TrainNumber} not found");
					}
				}
			}
		}

		private static async Task<string> BookAsync(Train train, CoachType[] coachTypes, string firstName, string lastName)
		{
			using (var client = new UzClient())
			{
				foreach (var coachType in coachTypes)
				{
					var coaches = await client.ListCoachesAsync(train, coachType);

					// TODO: Smart coach and seat selection algorythm
					foreach (var coach in coaches.OrderByDescending(c => c.PlacesCount))
					{
						var seats = await client.ListSeatsAsync(train, coach);

						foreach (var seat in seats)
						{
							try
							{
								await client.BookSeatAsync(train, coach, seat, firstName, lastName);
							}
							catch (ResponseException)
							{
								continue;
							}

							return client.GetSessionId();
						}
					}
				}
			}

			return null;
		}

		private static CoachType FindCoachType(Train train, string coachType)
		{
			return train.CoachTypes.FirstOrDefault(ct => ct.Letter == coachType);
		}
	}
}