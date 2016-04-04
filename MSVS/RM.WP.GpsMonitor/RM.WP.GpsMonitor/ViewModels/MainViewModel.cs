using System;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using RM.WP.GpsMonitor.Common;

namespace RM.WP.GpsMonitor.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		private readonly ILocationProvider _provider;
		private readonly CoreDispatcher _updateDispatcher;

		private bool _isLoading = true;
		private PositionStatus _status = PositionStatus.Initializing;
		private Location _location = Location.Empty;

		public MainViewModel(CoreDispatcher updateDispatcher, ILocationProvider locationProvider)
		{
			_updateDispatcher = updateDispatcher;
			_provider = locationProvider;

			if (locationProvider != null)
			{
				_provider.StatusChanged += OnProviderStatusChanged;
				_provider.PositionChanged += OnProviderPositionChanged;
			}
		}

		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				_isLoading = value;
				OnPropertyChanged();
			}
		}

		public string AppTitle => ResourceHelper.GetString("AppName").ToUpper();

		public PositionStatus Status
		{
			get { return _status; }
			set
			{
				_status = value;
				OnPropertyChanged();
			}
		}

		public Location Location
		{
			get { return _location; }
			set
			{
				IsLoading = false;

				_location = value;
				OnPropertyChanged();
			}
		}

		private async void OnProviderPositionChanged(ILocationProvider sender, EventArgs<Location> args)
		{
			if (_updateDispatcher != null)
			{
				await _updateDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Location = args.Data; });
			}
			else
			{
				Location = args.Data;
			}
		}

		private async void OnProviderStatusChanged(ILocationProvider sender, EventArgs<PositionStatus> args)
		{
			if (_updateDispatcher != null)
			{
				await _updateDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Status = args.Data; });
			}
			else
			{
				Status = args.Data;
			}
		}
	}

	public class FakeMainViewModel : MainViewModel
	{
		public FakeMainViewModel(CoreDispatcher updateDispatcher)
			: base(updateDispatcher, null)
		{
			DoReload();
		}

		private void DoReload()
		{
			var rng = new Random();

			Location = new Location(
								30.0 + rng.NextDouble(),
								50.0 + rng.NextDouble(),
								100.0 * rng.NextDouble(),
								3.0 + rng.NextDouble(),
								90.0 + 20.0 * (1.0 - rng.NextDouble()),
								12.0,
								16.0,
								PositionSource.Unknown
							);
		}
	}
}
