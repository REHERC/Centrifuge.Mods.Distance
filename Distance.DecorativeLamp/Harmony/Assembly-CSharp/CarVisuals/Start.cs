using HarmonyLib;

namespace Distance.DecorativeLamp.Harmony
{
	[HarmonyPatch(typeof(CarVisuals), "Start")]
	internal static class CarVisuals__Start
	{
		[HarmonyPostfix]
		internal static void Postfix(CarVisuals __instance)
		{
			__instance.gameObject.AddComponent<DecorativeLampLogic>();
		}
	}
}
