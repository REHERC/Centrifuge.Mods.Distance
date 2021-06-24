using Distance.AdventureMaker.Scripts.MainMenu;
using HarmonyLib;

namespace Distance.AdventureMaker.Harmony
{
	[HarmonyPatch(typeof(MainMenuLogic), "Awake")]
	internal static class MainMenuLogic__Awake
	{
		[HarmonyPostfix]
		internal static void Postfix(MainMenuLogic __instance)
		{
			var controller = __instance.gameObject.AddComponent<MainMenuButtonController>();
		}
	}
}