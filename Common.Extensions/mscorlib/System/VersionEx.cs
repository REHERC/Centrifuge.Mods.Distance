#pragma warning disable RCS1110, RCS1123
using System;

public static class VersionEx
{
	public static DateTime ToDate(this Version version)
	{
		long ticks = TimeSpan.TicksPerDay * version.Build + TimeSpan.TicksPerSecond * 2 * version.Revision;
		return new DateTime(2000, 1, 1).Add(new TimeSpan(ticks));
	}
}