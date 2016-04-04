using Windows.Devices.Geolocation;
using Windows.Foundation;
using RM.WP.GpsMonitor.Common;

namespace RM.WP.GpsMonitor.DataProviders
{
	internal sealed class GpsLocationProvider : ILocationProvider
	{
		private const PositionAccuracy _accuracy = PositionAccuracy.High;
		private const int _reportingInterval = 5 * 1000; // 5 seconds
		private const double _threshold = 10; // metres

		private readonly Geolocator _locator;

		private PositionStatus _lastStatus;
		private Location _lastPosition;

		public GpsLocationProvider()
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

		public PositionStatus LastStatus => _lastStatus;

		public Location LastPosition => _lastPosition;

		public event TypedEventHandler<ILocationProvider, EventArgs<PositionStatus>> StatusChanged;

		public event TypedEventHandler<ILocationProvider, EventArgs<Location>> PositionChanged;

		private void OnLocatorStatusChanged(Geolocator sender, StatusChangedEventArgs args)
		{
			_lastStatus = args.Status;
			StatusChanged?.Invoke(this, new EventArgs<PositionStatus>(_lastStatus));
		}

		private void OnLocatorPositionChanged(Geolocator sender, PositionChangedEventArgs args)
		{
			_lastPosition = Location.FromGeoposition(args.Position);
			PositionChanged?.Invoke(this, new EventArgs<Location>(_lastPosition));
		}
	}
}
