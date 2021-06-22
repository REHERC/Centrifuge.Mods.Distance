using Distance.AdventureMaker.Scripts.MainMenu;
using HarmonyLib;

namespace Distance.AdventureMaker.Harmony
{
	[HarmonyPatch(typeof(MainMenuLogic), "Start")]
	internal static class MainMenuLogic__Start
	{
		[HarmonyPrefix]
		internal static void Prefix(MainMenuLogic __instance)
		{
			__instance.gameObject.AddComponent<CampaignsMenuButton>();
		}
	}
}