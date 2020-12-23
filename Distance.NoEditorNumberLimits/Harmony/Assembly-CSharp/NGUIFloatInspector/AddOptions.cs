using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(NGUIFloatInspector), "AddOptions")]
	internal static class NGUIFloatInspector__AddOptions
	{
		[HarmonyPostfix]
		internal static void Postfix(NGUIFloatInspector __instance)
		{
			// REWRITE VALUES AFTER ORIGINAL CODE ANYWAY :^)

			__instance.SetMin(float.MinValue);
			__instance.SetMax(float.MaxValue);

			// "SHOULD" WORK
		}
	}
}
