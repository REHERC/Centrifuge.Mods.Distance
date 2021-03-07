using Distance.MenuUtilities.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Distance.MenuUtilities.Harmony
{
	[HarmonyPatch(typeof(LevelPlaylist), "Load")]
	internal static class LevelPlaylist__Load
	{
		[HarmonyPostfix]
		internal static void Postfix(ref GameObject __result, string levelPlaylistPath)
		{
			LevelPlaylistCompoundData data = __result.gameObject.AddComponent<LevelPlaylistCompoundData>();
			data.FilePath = levelPlaylistPath;
			data.Playlist = __result.GetComponent<LevelPlaylist>();
		}
	}
}
