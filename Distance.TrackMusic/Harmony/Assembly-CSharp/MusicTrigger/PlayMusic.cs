using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(MusicTrigger), "PlayMusic")]
	internal static class MusicTrigger__PlayMusic
	{
		[HarmonyPostfix]
		internal static void Postfix(MusicTrigger __instance)
		{
			SoundPlayerLogic soundPlayer = Mod.Instance.SoundPlayer;
			soundPlayer.PlayTrack(soundPlayer.GetMusicChoiceValue(__instance.gameObject, "Trigger"), 0f);
		}
	}
}
