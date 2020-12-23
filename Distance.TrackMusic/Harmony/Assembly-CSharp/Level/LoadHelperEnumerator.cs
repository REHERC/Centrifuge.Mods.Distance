using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(Level), "LoadHelperEnumerator")]
	internal static class Level__LoadHelperEnumerator
	{
		[HarmonyPrefix]
		internal static void Prefix(bool isWorkingStateLevel)
		{
			Mod.Instance.LevelEditor.IsWorkingStateLevel = isWorkingStateLevel;
		}
	}
}
