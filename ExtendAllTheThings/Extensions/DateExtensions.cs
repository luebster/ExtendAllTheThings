using System;

namespace ExtendAllTheThings.Extensions
{
	//TODO: refactor to use System.DateTime
	public static class DateExtensions
	{
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

		/// <summary>
		/// Determines if this date is a federal holiday.
		/// </summary>
		/// <param name="date">This date</param>
		/// <returns>True if this date is a federal holiday</returns>
		public static bool IsFederalHoliday(this DateTime date)
		{
			// to ease typing
			int nthWeekDay = (int)Math.Ceiling(date.Day / 7.0d);
			DayOfWeek dayName = date.DayOfWeek;
			bool isThursday = dayName == DayOfWeek.Thursday;
			bool isFriday = dayName == DayOfWeek.Friday;
			bool isMonday = dayName == DayOfWeek.Monday;
			bool isWeekend = dayName == DayOfWeek.Saturday || dayName == DayOfWeek.Sunday;

			// New Years Day (Jan 1, or preceding Friday/following Monday if weekend)
			if ((date.Month == 12 && date.Day == 31 && isFriday) ||
					(date.Month == 1 && date.Day == 1 && !isWeekend) ||
					(date.Month == 1 && date.Day == 2 && isMonday))
			{
				return true;
			}

			// MLK day (3rd monday in January)
			if (date.Month == 1 && isMonday && nthWeekDay == 3)
			{
				return true;
			}

			// President’s Day (3rd Monday in February)
			if (date.Month == 2 && isMonday && nthWeekDay == 3)
			{
				return true;
			}

			// Memorial Day (Last Monday in May)
			if (date.Month == 5 && isMonday && date.AddDays(7).Month == 6)
			{
				return true;
			}

			// Independence Day (July 4, or preceding Friday/following Monday if weekend)
			if ((date.Month == 7 && date.Day == 3 && isFriday) ||
					(date.Month == 7 && date.Day == 4 && !isWeekend) ||
					(date.Month == 7 && date.Day == 5 && isMonday))
			{
				return true;
			}

			// Labor Day (1st Monday in September)
			if (date.Month == 9 && isMonday && nthWeekDay == 1)
			{
				return true;
			}

			// Columbus Day (2nd Monday in October)
			if (date.Month == 10 && isMonday && nthWeekDay == 2)
			{
				return true;
			}

			// Veteran’s Day (November 11, or preceding Friday/following Monday if weekend))
			if ((date.Month == 11 && date.Day == 10 && isFriday) ||
					(date.Month == 11 && date.Day == 11 && !isWeekend) ||
					(date.Month == 11 && date.Day == 12 && isMonday))
			{
				return true;
			}

			// Thanksgiving Day (4th Thursday in November)
			if (date.Month == 11 && isThursday && nthWeekDay == 4)
			{
				return true;
			}

			// Christmas Day (December 25, or preceding Friday/following Monday if weekend))
			if ((date.Month == 12 && date.Day == 24 && isFriday) ||
					(date.Month == 12 && date.Day == 25 && !isWeekend) ||
					(date.Month == 12 && date.Day == 26 && isMonday))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Get Catholic easter for requested year
		/// </summary>
		/// <param name="year">Year of easter</param>
		/// <returns>DateTime of Catholic Easter</returns>
		public static DateTime GetCatholicEaster(this int Year)
		{
			{
				// Gauss Calculation
				////////////////////

				int Month = 3;

				// Determine the Golden number:
				int G = (Year % 19) + 1;

				// Determine the century number:
				int Century = (Year / 100) + 1;

				// Correct for the years who are not leap years:
				int X = (3 * Century / 4) - 12;

				// Moon correction:
				int Y = (((8 * Century) + 5) / 25) - 5;

				// Find Sunday:
				int Z = (5 * Year / 4) - X - 10;

				// Determine exact(age of moon on 1 January of that year(follows a cycle of 19 years)):
				int E = ((11 * G) + 20 + Y - X) % 30;
				if (E == 24) { E++; }
				if ((E == 25) && (G > 11)) { E++; }

				// Get the full moon:
				int N = 44 - E;
				if (N < 21) { N += 30; }

				// Up to Sunday:
				int P = N + 7 - ((Z + N) % 7);

				// Easter date:
				if (P > 31)
				{
					P -= 31;
					Month = 4;
				}
				return new DateTime(Year, Month, P);
			}
		}

		public static bool IsChristmasEve(this DateTime dateTimeRequested)
		{
			// they can pick a time up until 1pm (13:00:00)
			// therefore, we can match on any time after that.
			DateTime closingTime = new DateTime(dateTimeRequested.Year, 12, 24, 13, 0, 0);

			return dateTimeRequested.Month == 12 && dateTimeRequested.Day == 24 && dateTimeRequested > closingTime;
		}
	}
}