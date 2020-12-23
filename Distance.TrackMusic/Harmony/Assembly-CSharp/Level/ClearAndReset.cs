using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(Level), "ClearAndReset")]
	internal static class Level__ClearAndReset
	{
		[HarmonyPrefix]
		internal static void Prefix(Level __instance, bool destroyObjects)
		{
			if (destroyObjects && !Mod.Instance.LevelEditor.IsWorkingStateLevel)
			{
				Mod.Instance.LevelEditor.ResetLevelSettings(__instance.Settings_);
			}

			Mod.Instance.LevelEditor.IsWorkingStateLevel = false;
		}
	}
}
