using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(Options.MinInt), "Format")]
	internal static class Options_MinInt__Format
	{
		[HarmonyPrefix]
		internal static bool Prefix(ref string __result)
		{
			__result = string.Empty;
			return false;
		}
	}
}
