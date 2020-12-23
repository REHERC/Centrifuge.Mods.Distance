using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(MusicZone), "Visit")]
	internal static class MusicZone__Visit
	{
		[HarmonyPostfix]
		internal static void Postfix(MusicZone __instance, IVisitor visitor)
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
