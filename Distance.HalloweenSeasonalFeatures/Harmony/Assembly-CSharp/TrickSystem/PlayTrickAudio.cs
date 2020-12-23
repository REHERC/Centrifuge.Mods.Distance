using HarmonyLib;

namespace Distance.HalloweenSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(TrickSystem), "PlayTrickAudio")]
	internal class TrickSystem__PlayTrickAudio
	{
	}
}
