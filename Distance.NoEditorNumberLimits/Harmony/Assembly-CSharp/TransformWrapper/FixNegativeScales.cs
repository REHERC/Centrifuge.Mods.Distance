using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(TransformWrapper), "FixNegativeScales")]
	internal static class TransformWrapper__FixNegativeScales
	{
		[HarmonyPrefix]
		internal static bool Prefix()
		{
			return false;
		}
	}
}
