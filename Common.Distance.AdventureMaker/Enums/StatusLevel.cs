using System;

namespace Distance.AdventureMaker.Common.Enums
{
	[Flags]
	public enum StatusLevel
	{
		None = 0,
		Info = 1,
		Warning = 2,
		Error = 4,
		Any = Info | Warning | Error
	}
}
