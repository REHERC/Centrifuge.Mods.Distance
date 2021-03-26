using System;

namespace Distance.AdventureMaker.Common.Enums
{
	[Flags]
	public enum LevelTransitionType : byte
	{
		None,
		Default,
		LostToEchoes,
		EarlyAccess
	}
}
