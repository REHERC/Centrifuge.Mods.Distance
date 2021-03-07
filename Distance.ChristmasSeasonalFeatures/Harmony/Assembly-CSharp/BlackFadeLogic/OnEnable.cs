using HarmonyLib;

namespace Distance.ChristmasSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(BlackFadeLogic), "OnEnable")]
	internal static class BlackFadeLogic__OnEnable
	{
		[HarmonyPostfix]
		internal static void Postfix(BlackFadeLogic __instance)
		{
			Mod.Instance.LoadingScreenTextures.SetDefaulttextures(__instance.loadingTextures_);
			//__instance.loadingTextures_ = Mod.Instance.LoadingScreenTextures.Textures;
		}
	}
}
