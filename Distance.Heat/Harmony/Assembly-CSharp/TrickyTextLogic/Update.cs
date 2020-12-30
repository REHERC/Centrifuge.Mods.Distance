using Distance.Heat.Enums;
using HarmonyLib;

namespace Distance.Heat.Harmony
{
	[HarmonyPatch(typeof(TrickyTextLogic), "Update")]
	internal static class TrickyTextLogic__Update
	{
		[HarmonyPostfix]
		internal static void Postfix(TrickyTextLogic __instance)
		{
			if (Mod.Instance.DisplayCondition && Mod.Instance.Config.DisplayMode == DisplayMode.Hud)
			{
				__instance.SetAlpha(1);
				__instance.textMesh_.text = Mod.Instance.Text;
			}
		}
	}
}
