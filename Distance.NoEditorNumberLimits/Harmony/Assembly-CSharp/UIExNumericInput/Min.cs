using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(UIExNumericInput), "Min_", MethodType.Setter)]
	internal static class UIExNumericInput__Min__set
	{
		[HarmonyPrefix]
		internal static void Postfix(UIExNumericInput __instance)
		{
			__instance.min_ = float.MinValue;
		}
	}
}
