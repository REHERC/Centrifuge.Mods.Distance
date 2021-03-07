using HarmonyLib;

namespace Distance.HalloweenSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(CarLogic), "Awake")]
	internal static class CarLogic__Awake
	{
		[HarmonyPostfix]
		internal static void Postfix(CarLogic __instance)
		{
			__instance.gameObject.AddComponent<SkeletonCorruptionLogic>();
		}
	}
}
