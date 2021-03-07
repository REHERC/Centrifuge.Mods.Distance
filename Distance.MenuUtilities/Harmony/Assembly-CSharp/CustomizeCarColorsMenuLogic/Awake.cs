using Distance.MenuUtilities.Scripts;
using HarmonyLib;

namespace Distance.MenuUtilities.Harmony
{
	[HarmonyPatch(typeof(CustomizeCarColorsMenuLogic), "Awake")]
	internal static class CustomizeCarColorsMenuLogic__Awake
	{
		[HarmonyPostfix]
		internal static void Postfix(CustomizeCarColorsMenuLogic __instance)
		{
			CustomizeMenuCompoundData data = __instance.gameObject.AddComponent<CustomizeMenuCompoundData>();
			data.Menu = __instance;
		}
	}
}
