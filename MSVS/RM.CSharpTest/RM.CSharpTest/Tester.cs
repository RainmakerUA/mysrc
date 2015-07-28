using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using static System.Console;

namespace RM.CSharpTest
{
	internal class Tester
	{
		private readonly ConcurrentQueue<string> _testQueue;
		private volatile CancellationTokenSource _ctoken;

		public Tester(string name, string email)
		{
			_testQueue = new ConcurrentQueue<string>();

			Name = name;
			EMail = email;
		}

		public string Name { get; }

		public string EMail { get; }

		public int TestedCount { get; private set; }

		public void AddTests(IEnumerable<string> tests)
		{
			if (_ctoken != null)
			{
				Stop();
			}

			foreach (var item in tests)
			{
				_testQueue.Enqueue(item);
            }
		}

		public async void Start()
		{
			_ctoken = new CancellationTokenSource();

			WriteLine("Testing is started");
			TestedCount = 0;

			await Task.Run(() => {
								string test;
								while (_ctoken != null && !_ctoken.Token.IsCancellationRequested
											&& _testQueue.TryDequeue(out test))
								{
									var task = DoTest(test);
									task.Wait();

									TestedCount++;
								}

								_ctoken = null;
							},
						_ctoken.Token
					);

			TestingCompleted?.Invoke(this, $"{Name} finished job");
		}

		public void Stop()
		{
			_ctoken?.Cancel();
			WriteLine($"Testing is stopped. Tests completed: {TestedCount}");
		}

		public event EventHandler<string> TestingCompleted;

		private static Task DoTest(string test)
		{
			if (String.IsNullOrEmpty(test))
			{
				throw new ArgumentException($"Argument'{nameof(test)}' cannot be null or empty!", nameof(test));
			}

			return Task.Run(() => {
									var duration = new Random().Next(1000, 3001);
                                    Thread.Sleep(duration);
									WriteLine($"Test finished: {test}, lasted: {duration} ms");
								});
		}
	}
}
