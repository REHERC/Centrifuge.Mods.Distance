namespace Distance.ChristmasSeasonalFeatures
{
	public enum ActivationMode : byte
	{
		Always,
		DuringDecember,
		First25DaysOfDecember,
		DuringWeekOfChristmas,
		OnlyOnDecember24And25,
		AlwaysExceptDecember,
		Never
	}
}
