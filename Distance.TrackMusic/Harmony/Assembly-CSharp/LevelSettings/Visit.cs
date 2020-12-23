using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(LevelSettings), "Visit")]
	internal static class LevelSettings__Visit
	{
		[HarmonyPostfix]
		internal static void Postfix(LevelSettings __instance, IVisitor visitor)
		{
			SoundPlayerLogic soundPlayer = Mod.Instance.SoundPlayer;
			LevelEditorLogic levelEditor = Mod.Instance.LevelEditor;

			soundPlayer.DownloadAllTracks();

			if (!(visitor is Serializers.Serializer) && !(visitor is Serializers.Deserializer))
			{
				visitor.VisitAction("Toggle Custom Music", levelEditor.ToggleCustomMusic, null);
				visitor.VisitAction("Add Music Track", levelEditor.AddMusicTrack, null);

				if (!__instance.HasComponent<ZEventListener>())
				{
					visitor.VisitAction("Set Music Choice", levelEditor.AddMusicChoiceLevelSettings, null);
				}

				soundPlayer.PlayTrack(soundPlayer.GetMusicChoiceValue(__instance.gameObject, "Level"), 2000f);
			}
		}
	}
}
