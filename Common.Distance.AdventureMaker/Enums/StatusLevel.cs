using System;

namespace Distance.AdventureMaker.Common.Enums
{
	[Flags]
	public enum StatusLevel
	{
		MSG,
		WRN,
		ERR,
		Any = MSG | WRN | ERR
	}
}
