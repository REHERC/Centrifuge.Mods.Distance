using System;

namespace Distance.AdventureMaker.Common.Enums
{
	[Flags]
	public enum ResourceType : byte
	{
		None,
		Text,
		Level,
		Texture,
		Music,
		Assets
	}
}
