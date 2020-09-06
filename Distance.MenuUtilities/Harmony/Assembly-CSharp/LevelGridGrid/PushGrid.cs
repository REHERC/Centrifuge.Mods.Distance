using Distance.MenuUtilities.Scripts;
using HarmonyLib;
using LevelEditorTools;

namespace Distance.MenuUtilities.Harmony
{
    [HarmonyPatch(typeof(LevelGridGrid), "PushGrid")]
    internal class LevelGridGrid__PushGrid
    {
        [HarmonyPostfix]
        internal static void Postfix(LevelGridGrid __instance)
        {
            LevelPlaylist playlist = __instance.playlist_;

            LevelPlaylistCompoundData data = playlist.GetComponent<LevelPlaylistCompoundData>();

            if (data && !playlist.IsResourcesPlaylist())
            {
                G.Sys.MenuPanelManager_.SetBottomLeftActionButton(InternalResources.Constants.DELETE_PLAYLIST_INPUT, "DELETE PLAYLIST");
            }
        }
    }
}
