using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(Options.MaxFloat), "Format")]
	internal static class Options_MaxFloat__Format
	{
		[HarmonyPrefix]
		internal static bool Prefix(ref string __result)
		{
			__result = string.Empty;
			return false;
		}
	}
}
