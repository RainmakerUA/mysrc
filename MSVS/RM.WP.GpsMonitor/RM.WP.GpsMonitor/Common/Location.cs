using System;
using Windows.Devices.Geolocation;

namespace RM.WP.GpsMonitor.Common
{
	public sealed class Location
	{
		private static readonly Location _empty;

		private readonly double _latitude;
		private readonly double _longitude;
		private readonly double _altitude;
		private readonly double _speed;
		private readonly double _heading;
		private readonly double _accuracy;
		private readonly double _altitudeAccuracy;
		private readonly PositionSource _positionSource;

		static Location()
		{
			_empty = new Location(
							Double.NaN, Double.NaN, Double.NaN,
							Double.NaN, Double.NaN, Double.NaN,
							Double.NaN, PositionSource.Unknown
						);
		}

		internal Location(double latitude, double longitude, double altitude, double speed, double heading, double accuracy,
											double altitudeAccuracy, PositionSource positionSource)
		{
			_latitude = latitude;
			_longitude = longitude;
			_altitude = altitude;
			_speed = speed;
			_heading = heading;
			_accuracy = accuracy;
			_altitudeAccuracy = altitudeAccuracy;
			_positionSource = positionSource;
		}
		
		public double Latitude
		{
			get { return _latitude; }
		}

		public double Longitude
		{
			get { return _longitude; }
		}

		public double Altitude
		{
			get { return _altitude; }
		}

		public double Speed
		{
			get { return _speed; }
		}

		public double Heading
		{
			get { return _heading; }
		}

		public double Accuracy
		{
			get { return _accuracy; }
		}

		public double AltitudeAccuracy
		{
			get { return _altitudeAccuracy; }
		}

		public PositionSource Source
		{
			get { return _positionSource; }
		}

		public static Location Empty
		{
			get { return _empty; }
		}

		public static Location FromGeoposition(Geoposition position)
		{
			var coordinate = position.Coordinate;
			var basicPosition = coordinate.Point.Position;

			return new Location(
								basicPosition.Latitude,
								basicPosition.Longitude,
								basicPosition.Altitude,
								coordinate.Speed.GetValueOrDefault(Double.NaN),
								coordinate.Heading.GetValueOrDefault(Double.NaN),
								coordinate.Accuracy,
								coordinate.AltitudeAccuracy.GetValueOrDefault(Double.NaN),
								coordinate.PositionSource
							);
		}
	}
}
