using HarmonyLib;

namespace Distance.HalloweenSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(MainMenuLogic), "Awake")]
	internal class MainMenuLogic__Awake
	{
		[HarmonyPostfix]
		internal static void Postfix(MainMenuLogic __instance)
		{
			__instance.gameObject.AddComponent<HalloweenAudioLogic>();
		}
	}
}
