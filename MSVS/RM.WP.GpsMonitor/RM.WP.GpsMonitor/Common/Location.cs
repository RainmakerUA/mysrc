using System;
using Windows.Devices.Geolocation;

namespace RM.WP.GpsMonitor.Common
{
	public sealed class Location
	{
		private const double _nan = Double.NaN;

		static Location()
		{
			Empty = new Location(
							_nan, _nan, _nan,
							_nan, _nan, _nan,
							_nan, PositionSource.Unknown
						);
		}

		internal Location(double latitude, double longitude, double altitude, double speed, double heading, double accuracy,
											double altitudeAccuracy, PositionSource positionSource)
		{
			Latitude = latitude;
			Longitude = longitude;
			Altitude = altitude;
			Speed = speed;
			Heading = heading;
			Accuracy = accuracy;
			AltitudeAccuracy = altitudeAccuracy;
			Source = positionSource;
		}
		
		public double Latitude { get; }

		public double Longitude { get; }

		public double Altitude { get; }

		public double Speed { get; }

		public double Heading { get; }

		public double Accuracy { get; }

		public double AltitudeAccuracy { get; }

		public PositionSource Source { get; }

		public static Location Empty { get; }

		public static Location FromGeoposition(Geoposition position)
		{
			var coordinate = position.Coordinate;
			var basicPosition = coordinate.Point.Position;

			return new Location(
								basicPosition.Latitude,
								basicPosition.Longitude,
								basicPosition.Altitude,
								coordinate.Speed.GetValueOrDefault(_nan),
								coordinate.Heading.GetValueOrDefault(_nan),
								coordinate.Accuracy,
								coordinate.AltitudeAccuracy.GetValueOrDefault(_nan),
								coordinate.PositionSource
							);
		}
	}
}
