using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(AudioEventTrigger), "PlayMusic")]
	internal static class AudioEventTrigger__PlayMusic
	{
		[HarmonyPostfix]
		internal static void Postfix(AudioEventTrigger __instance)
		{
			SoundPlayerLogic soundPlayer = Mod.Instance.SoundPlayer;
			soundPlayer.PlayTrack(soundPlayer.GetMusicChoiceValue(__instance.gameObject, "Trigger"), 0f);
		}
	}
}
