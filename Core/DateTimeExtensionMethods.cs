using System;

namespace AD
{
	public static class DateTimeExtensionMethods
	{
		// See: http://stackoverflow.com/questions/38039/how-can-i-get-the-datetime-for-the-start-of-the-week
		public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			int diff = dt.DayOfWeek - startOfWeek;
			if (diff < 0)
			{
				diff += 7;
			}
			return dt.AddDays(-1 * diff).Date;
		}
	}
}

