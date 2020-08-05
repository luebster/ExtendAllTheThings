using System;

namespace ExtendAllTheThings
{
	public static class Dates
	{
		/// <summary>
		/// A timestamp localized to a given time zone.
		/// </summary>
		/// <param name="TimeZoneID"></param>
		/// <returns></returns>
		public static DateTime Now(string TimeZoneID)
		{
			DateTime utcTime = DateTime.UtcNow;
			var tzi = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneID);
			return TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
		}

		public static DateTime Now()
		{
			return Now("Eastern Standard Time");
		}

		/// <summary>
		/// Ex: August 12, 2020
		/// </summary>
		/// <param name="date"></param>
		/// <returns>A date with full month, date, and year</returns>
		public static string ToShortDecriptionString(this DateTime date)
		{
			return date.ToString("MMMM dd, yyyy");
		}

		/// <summary>
		/// Date and time as one string
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string ToShortDateTimeString(this DateTime date)
		{
			return string.Format("{0} {1}", date.ToShortDateString(), date.ToShortTimeString());
		}
	}
}