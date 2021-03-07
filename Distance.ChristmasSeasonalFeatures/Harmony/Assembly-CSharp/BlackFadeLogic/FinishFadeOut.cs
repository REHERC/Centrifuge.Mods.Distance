using HarmonyLib;

namespace Distance.ChristmasSeasonalFeatures.Harmony.Assembly_CSharp
{
	[HarmonyPatch(typeof(BlackFadeLogic), "FinishFadeOut")]
	internal static class BlackFadeLogic__FinishFadeOut
	{
		[HarmonyPrefix]
		internal static void Prefix(BlackFadeLogic __instance)
		{
			if (Mod.Instance.IsActive && Mod.Instance.Config.OverrideLoadingScreens)
			{
				__instance.loadingTextures_ = Mod.Instance.LoadingScreenTextures.Textures;
			}
			else
			{
				__instance.loadingTextures_ = Mod.Instance.LoadingScreenTextures.DefaultTextures;
			}
		}
	}
}
