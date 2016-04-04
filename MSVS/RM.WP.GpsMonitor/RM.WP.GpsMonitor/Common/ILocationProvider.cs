using Windows.Devices.Geolocation;
using Windows.Foundation;
using RM.WP.GpsMonitor.DataProviders;

namespace RM.WP.GpsMonitor.Common
{
	public interface ILocationProvider
	{
		PositionStatus LastStatus { get; }
		Location LastPosition { get; }
		event TypedEventHandler<ILocationProvider, EventArgs<PositionStatus>> StatusChanged;
		event TypedEventHandler<ILocationProvider, EventArgs<Location>> PositionChanged;
	}
}
