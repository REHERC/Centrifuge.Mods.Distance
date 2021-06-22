using HarmonyLib;

namespace Distance.AdventureMaker.Harmony
{
	[HarmonyPatch(typeof(SteamworksUGC), "OnEnable")]
	internal static class SteamworksUGC__OnEnable
	{
		[HarmonyPrefix]
		internal static void Prefix(SteamworksUGC __instance)
		{
			//Mod.Instance.Logger.Error("THIS IS A TEST");
		}
	}
}