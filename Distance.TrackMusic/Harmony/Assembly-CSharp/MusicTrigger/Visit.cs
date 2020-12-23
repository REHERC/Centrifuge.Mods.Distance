using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(MusicTrigger), "Visit")]
	internal static class MusicTrigger__Visit
	{
		[HarmonyPostfix]
		internal static void Postfix(MusicTrigger __instance, IVisitor visitor)
		{
			LevelEditorLogic levelEditor = Mod.Instance.LevelEditor;

			if (visitor is NGUIComponentInspector)
			{
				if (!__instance.HasComponent<ZEventListener>())
				{
					visitor.VisitAction("Set Music Choice", levelEditor.AddMusicChoiceSelection, null);
				}
			}
		}
	}
}
