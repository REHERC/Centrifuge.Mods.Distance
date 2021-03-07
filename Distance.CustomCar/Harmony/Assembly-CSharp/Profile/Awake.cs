using HarmonyLib;

namespace Distance.CustomCar.Harmony
{
	[HarmonyPatch(typeof(Profile), "Awake")]
	internal static class Profile__Awake
	{
		[HarmonyPostfix]
		internal static void Postfix(Profile __instance)
		{
			CarColors[] carColors = new CarColors[G.Sys.ProfileManager_.carInfos_.Length];
			for (int colorIndex = 0; colorIndex < carColors.Length; colorIndex++)
			{
				carColors[colorIndex] = G.Sys.ProfileManager_.carInfos_[colorIndex].colors_;
			}

			__instance.carColorsList_ = carColors;
		}
	}
}