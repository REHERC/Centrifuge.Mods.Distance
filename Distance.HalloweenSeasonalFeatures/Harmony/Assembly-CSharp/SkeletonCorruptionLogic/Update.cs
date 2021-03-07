using HarmonyLib;

namespace Distance.HalloweenSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(SkeletonCorruptionLogic), "Update")]
	internal static class SkeletonCorruptionLogic__Update
	{
		[HarmonyPrefix]
		internal static void Prefix(SkeletonCorruptionLogic __instance)
		{
			CutPlaneShaderController cutPlane = __instance.gameObject.GetComponent<CutPlaneShaderController>();

			__instance.IsCorrupted_ = cutPlane.CorruptionEffectActive_;
		}
	}
}
