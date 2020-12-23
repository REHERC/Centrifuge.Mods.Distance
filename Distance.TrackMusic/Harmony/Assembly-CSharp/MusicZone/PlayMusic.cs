using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(MusicZone), "PlayMusic")]
	internal static class MusicZone__PlayMusic
	{
		[HarmonyPostfix]
		internal static void Postfix(MusicZone __instance)
		{
			SoundPlayerLogic soundPlayer = Mod.Instance.SoundPlayer;
			soundPlayer.PlayTrack(soundPlayer.GetMusicChoiceValue(__instance.gameObject, "Zone"), 0f);
		}
	}
}
