using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(UIExNumericInput), "Max_", MethodType.Setter)]
	internal static class UIExNumericInput__Max__set
	{
		[HarmonyPrefix]
		internal static void Postfix(UIExNumericInput __instance)
		{
			__instance.max_ = float.MaxValue;
		}
	}
}
