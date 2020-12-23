using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(Options.MinFloat), "Format")]
	internal static class Options_MinFloat__Format
	{
		[HarmonyPrefix]
		internal static bool Prefix(ref string __result)
		{
			__result = string.Empty;
			return false;
		}
	}
}
