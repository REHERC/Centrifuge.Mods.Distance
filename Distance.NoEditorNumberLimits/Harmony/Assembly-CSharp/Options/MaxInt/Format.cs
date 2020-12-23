using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(Options.MaxInt), "Format")]
	internal static class Options_MaxInt__Format
	{
		[HarmonyPrefix]
		internal static bool Prefix(ref string __result)
		{
			__result = string.Empty;
			return false;
		}
	}
}
