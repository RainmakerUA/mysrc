using Windows.Devices.Geolocation;
using Windows.Foundation;
using RM.WP.GpsMonitor.Common;

namespace RM.WP.GpsMonitor.DataProviders
{
	internal sealed class LocationProvider
	{
		private const PositionAccuracy _accuracy = PositionAccuracy.High;
		private const int _reportingInterval = 5 * 1000; // 5 seconds
		private const double _threshold = 10; // metres

		private readonly Geolocator _locator;

		private PositionStatus _lastStatus;
		private Location _lastPosition;

		public LocationProvider()
		{
			_locator = new Geolocator
							{
								DesiredAccuracy = _accuracy,
								ReportInterval = _reportingInterval,
								MovementThreshold = _threshold
							};
			_locator.StatusChanged += OnLocatorStatusChanged;
			_locator.PositionChanged += OnLocatorPositionChanged;
		}

		public PositionStatus LastStatus
		{
			get { return _lastStatus; }
		}

		public Location LastPosition
		{
			get { return _lastPosition; }
		}

		public event TypedEventHandler<LocationProvider, EventArgs<PositionStatus>> StatusChanged;

		public event TypedEventHandler<LocationProvider, EventArgs<Location>> PositionChanged;

		private void OnLocatorStatusChanged(Geolocator sender, StatusChangedEventArgs args)
		{
			var handler = StatusChanged;

			_lastStatus = args.Status;

			if (handler != null)
			{
				handler(this, new EventArgs<PositionStatus>(_lastStatus));
			}
		}

		private void OnLocatorPositionChanged(Geolocator sender, PositionChangedEventArgs args)
		{
			var handler = PositionChanged;

			_lastPosition = Location.FromGeoposition(args.Position);

			if (handler != null)
			{
				handler(this, new EventArgs<Location>(_lastPosition));
			}
		}
	}
}
