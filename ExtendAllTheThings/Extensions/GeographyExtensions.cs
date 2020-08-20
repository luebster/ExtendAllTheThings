using GeoCoordinatePortable;

using System;

namespace ExtendAllTheThings.Extensions
{
	public static class GeographyExtensions
	{
		public static double GetDistance(this GeoCoordinate geo, double latitude, double longitude, char unit = 'm', int decimalPlaces = 2)
		{
			var point = new GeoCoordinate(latitude, longitude);
			var distance = geo.GetDistanceTo(point); // this is in meters;

			switch (unit)
			{
				case 'm':
					distance /= 1609.34;
					break;

				case 'k':
					distance /= 1000;
					break;
			}

			return Math.Round(distance, decimalPlaces);
		}
	}
}