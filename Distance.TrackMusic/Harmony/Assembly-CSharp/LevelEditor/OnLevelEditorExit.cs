using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(LevelEditor), "OnLevelEditorExit")]
	internal static class LevelEditor__OnLevelEditorExit
	{
		[HarmonyPostfix]
		internal static void Postfix()
		{
			if (AudioManager.AllowCustomMusic_)
			{
				SoundPlayerLogic soundPlayer = Mod.Instance.SoundPlayer;
				soundPlayer.DownloadAllTracks();
				soundPlayer.PlayTrack(soundPlayer.GetMusicChoiceValue(G.Sys.GameManager_.LevelSettings_.gameObject, "Level"), 0f);
			}
		}
	}
}
