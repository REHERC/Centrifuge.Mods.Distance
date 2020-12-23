using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(AudioManager), "OnEventPostLoad")]
	internal static class AudioManager__OnEventPostLoad
	{
		[HarmonyPostfix]
		internal static void Postfix()
		{
			if (AudioManager.AllowCustomMusic_)
			{
				SoundPlayerLogic soundPlayer = Mod.Instance.SoundPlayer;

				soundPlayer.DownloadAllTracks();

				Mod.Instance.Logger.Info($"Trying to play {soundPlayer.GetMusicChoiceValue(G.Sys.GameManager_.LevelSettings_.gameObject, "Level")}");

				soundPlayer.PlayTrack(soundPlayer.GetMusicChoiceValue(G.Sys.GameManager_.LevelSettings_.gameObject, "Level"), 2000f, true);
			}
		}
	}
}
