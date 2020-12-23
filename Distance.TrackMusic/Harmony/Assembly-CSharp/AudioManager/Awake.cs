using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(AudioManager), "Awake")]
	internal static class AudioManager__Awake
	{
		[HarmonyPostfix]
		internal static void Postfix()
		{
			Mod.Instance.PatchPostLoad(false);
		}
	}
}
