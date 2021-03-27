using System;

namespace Distance.AdventureMaker.Common.Enums
{
	[Flags]
	public enum ResourceType : short
	{
		None = -1,
		Level = 0,
		Texture = 1,
		//Audio,
		//UnityAsset,
		//Assembly,
	}
}
