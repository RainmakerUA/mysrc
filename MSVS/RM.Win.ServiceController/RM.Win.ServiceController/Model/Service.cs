using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using RM.Lib.Wpf.Common.Commands;
using RM.Lib.Wpf.Common.ViewModel;
using RM.Win.ServiceController.Common;

namespace RM.Win.ServiceController.Model
{
	public class Service : BindableBase
	{
		private const string _noStatus = "(no status text)";

		private static readonly TimeSpan _waitTimeout = TimeSpan.FromMinutes(1);

		private static readonly ConcurrentDictionary<string, Service> _services = new();

		private static readonly Action<Service?> _startExecute = ExecuteStartCommand;
		private static readonly Func<Service?, bool> _startCanExecute = CanExecuteStartCommand;

		private static readonly Action<Service?> _stopExecute = ExecuteStopCommand;
		private static readonly Func<Service?, bool> _stopCanExecute = CanExecuteStopCommand;

		private static readonly Action<Service?> _restartExecute = ExecuteRestartCommand;
		private static readonly Func<Service?, bool> _restartCanExecute = CanExecuteRestartCommand;

		private static DispatcherTimer? _timer;
		private static Dispatcher? _dispatcher;

		private readonly DelegateCommand<Service> _startCommand;
		private readonly DelegateCommand<Service> _stopCommand;
		private readonly DelegateCommand<Service> _restartCommand;

		private readonly string _serviceName;

		private System.ServiceProcess.ServiceController? _controller;
		private string? _displayName;
		private ServiceControllerStatus? _status;
		private string? _errorText;
		private bool _isEnabled;

		public Service(string serviceName)
		{
			_serviceName = serviceName;

			_restartCommand = new DelegateCommand<Service>(_restartExecute, _restartCanExecute);
			_stopCommand = new DelegateCommand<Service>(_stopExecute, _stopCanExecute);
			_startCommand = new DelegateCommand<Service>(_startExecute, _startCanExecute);

			if (_services.TryAdd(_serviceName, this))
			{
				Refresh();
			}
			else
			{
				_displayName = _serviceName;
				_errorText = "Duplicate service in list";
			}
		}

		public ICommand StartCommand => _startCommand;

		public ICommand StopCommand => _stopCommand;

		public ICommand RestartCommand => _restartCommand;

		public bool IsValid => _controller != null;

		public bool IsEnabled
		{
			get => IsValid && _isEnabled;
			set
			{
				SetProperty(ref _isEnabled, IsValid && value);
				_startCommand.RaiseCanExecuteChanged();
				_stopCommand.RaiseCanExecuteChanged();
				_restartCommand.RaiseCanExecuteChanged();

				SetServiceEnabled?.Invoke(_serviceName, _isEnabled);
			}
		}

		public string? DisplayName
		{
			get => _displayName;
			private set => SetProperty(ref _displayName, value);
		}

		public ServiceControllerStatus? Status
		{
			get => _status;
			private set
			{
				if (SetProperty(ref _status, value))
				{
					OnPropertyChanged(nameof(StatusText));
				}
				
				_startCommand.RaiseCanExecuteChanged();
			}
		}

		public string StatusText => _status?.ToDisplayText() ?? _errorText ?? _noStatus;

		public static Dispatcher? Dispatcher
		{
			get => _dispatcher;
			set
			{
				if (_timer != null)
				{
					throw new InvalidOperationException("Dispatcher cannot be changed after timer is created");
				}

				_dispatcher = value;
			}
		}

		public static Action<string, bool>? SetServiceEnabled { get; set; }

		public static Action<Exception?>? ErrorAction { get; set; }

		public static Task StartAllAsync()
		{
			return DoForEachAsync(_services.Values, svc => svc.StartAsync());
		}

		public static Task StopAllAsync()
		{
			return DoForEachAsync(_services.Values, svc => svc.StopAsync());
		}

		public static Task RestartAllAsync()
		{
			return DoForEachAsync(_services.Values, svc => svc.RestartAsync());
		}

		public static void Reset()
		{
			_timer?.Stop();
			_timer = null;
			_services.Clear();
		}

		public static void UpdateTimer(TimeSpan interval)
		{
			if (_timer == null)
			{
				_timer = new DispatcherTimer(
										interval,
										DispatcherPriority.Background,
										OnRefreshTimerTick,
										Dispatcher ?? Dispatcher.CurrentDispatcher
									);
			}
			else
			{
				_timer.Stop();
				_timer.Interval = interval;
				_timer.Start();
			}
		}

		private void Refresh()
		{
			string? displayName = null;
			ServiceControllerStatus? status = null;

			try
			{
				if (_controller == null)
				{
					_controller = new System.ServiceProcess.ServiceController(_serviceName);
				}
				else
				{
					_controller.Refresh();
				}

				displayName = _controller.DisplayName;
				status = _controller.Status;
			}
			catch (Exception e)
			{
				_controller?.Dispose();
				_controller = null;
				_errorText = e.Message;
			}

			DisplayName = displayName ?? $"Error: {_serviceName}";
			Status = status;

			OnPropertyChanged(nameof(IsValid));
			OnPropertyChanged(nameof(IsEnabled));

			_startCommand.RaiseCanExecuteChanged();
			_stopCommand.RaiseCanExecuteChanged();
			_restartCommand.RaiseCanExecuteChanged();
		}

		private bool Start()
		{
			if (_controller != null)
			{
				_controller.Refresh();

				if (CanExecuteStartCommand(this))
				{
					_controller.Start();
					_controller.WaitForStatus(ServiceControllerStatus.Running, _waitTimeout);
					return true;
				}
			}

			return false;
		}

		private bool Stop()
		{
			if (_controller != null)
			{
				_controller.Refresh();

				if (CanExecuteStopCommand(this))
				{
					_controller.Stop();
					_controller.WaitForStatus(ServiceControllerStatus.Stopped, _waitTimeout);
					return true;
				}
			}

			return false;
		}

		private bool Restart()
		{
			Stop();
			Start();
			return true;
		}

		private Task<bool> StartAsync(CancellationToken cancellation = default)
		{
			return Task.Run(Start, cancellation);
		}

		private Task<bool> StopAsync(CancellationToken cancellation = default)
		{
			return Task.Run(Stop, cancellation);
		}

		private Task RestartAsync(CancellationToken cancellation = default)
		{
			return Task.Run(Restart, cancellation);
		}

		private static void OnRefreshTimerTick(object? sender, EventArgs e)
		{
			foreach (var service in _services.Values)
			{
				service.Refresh();
			}
		}

		private static Task DoForEachAsync<T>(IEnumerable<T> enumerable, Func<T, Task> actionAsync)
		{
			return Task.WhenAll(enumerable.Select(actionAsync));
		}

		private static void ExecuteStartCommand(Service? service)
		{
			service?.StartAsync().Catch(HandleException);
		}

		private static bool CanExecuteStartCommand(Service? service)
		{
			var status = service?._controller?.Status ?? default;
			return service is { IsEnabled: true } && status != ServiceControllerStatus.Running
													&& status != ServiceControllerStatus.StartPending
													&& status != ServiceControllerStatus.ContinuePending;
		}

		private static void ExecuteStopCommand(Service? service)
		{
			service?.StopAsync().Catch(HandleException);
		}

		private static bool CanExecuteStopCommand(Service? service)
		{
			var status = service?._controller?.Status ?? default;
			return service is { IsEnabled: true } && status != ServiceControllerStatus.Stopped
													&& status != ServiceControllerStatus.StopPending;
		}

		private static void ExecuteRestartCommand(Service? service)
		{
			service?.RestartAsync().Catch(HandleException);
		}

		private static bool CanExecuteRestartCommand(Service? service)
		{
			return CanExecuteStopCommand(service) || CanExecuteStartCommand(service);
		}

		private static void HandleException(Exception? exception)
		{
			ErrorAction?.Invoke(exception);
		}
	}
}
