using System;
using System.Globalization;

namespace Distance.ChristmasSeasonalFeatures
{
	internal static class ModActivationLogic
	{
		// Determines if the mod should be activated
		internal static bool IsModActive()
		{
			DateTime now;
			if (Mod.Instance.Config.TimeFormat == TimeFormat.Local)
			{
				now = DateTime.Now;
			}
			else
			{
				now = DateTime.UtcNow;
			}

			ActivationMode mode = Mod.Instance.Config.ActivationMode;

			switch (mode)
			{
				case ActivationMode.Always:
					return true;
				case ActivationMode.Never:
					return false;
				case ActivationMode.DuringDecember:
					return now.Month == 12;
				case ActivationMode.AlwaysExceptDecember:
					return now.Month != 12;
				case ActivationMode.First25DaysOfDecember:
					return now.Month == 12 && now.Day <= 25;
				case ActivationMode.OnlyOnDecember24And25:
					return now.Month == 12 && (now.Day == 24 || now.Day == 25);
				case ActivationMode.DuringWeekOfChristmas:
					Calendar cal = CultureInfo.CurrentCulture.Calendar;
					DateTime christmas = new DateTime(now.Year, now.Month, 25);

					int weekNow = cal.GetWeekOfYear(now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
					int weekChristmas = cal.GetWeekOfYear(christmas, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

					return weekNow == weekChristmas;
				default:
					return false; // Failsafe
			}
		}
	}
}
