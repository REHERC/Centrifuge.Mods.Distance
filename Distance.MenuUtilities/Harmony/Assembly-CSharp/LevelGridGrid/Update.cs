using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Distance.MenuUtilities.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Distance.MenuUtilities.Harmony
{
	[HarmonyPatch(typeof(LevelGridGrid), "Update")]
	internal static class LevelGridGrid__UpdatePageButtons
	{
		[HarmonyPostfix]
		internal static void Postfix(LevelGridGrid __instance)
		{
			if (!Mod.Instance.Config.EnableDeletePlaylistButton)
			{
				return;
			}

			LevelPlaylist playlist = __instance.playlist_;

			LevelPlaylistCompoundData data = playlist.GetComponent<LevelPlaylistCompoundData>();

			if (data && !playlist.IsResourcesPlaylist() && G.Sys.InputManager_.GetKeyUp(InternalResources.Constants.INPUT_DELETE_PLAYLIST))
			{
				MessageBox.Create($"Are you sure you want to remove [u]{playlist.Name_}[/u]?", "DELETE PLAYLIST")
				.SetButtons(MessageButtons.YesNo)
				.OnConfirm(() =>
				{
					try
					{
						FileEx.Delete(data.FilePath);
						playlist.Destroy();
						Object.DestroyImmediate(data.gameObject);
					}
					catch (System.Exception e)
					{
						Mod.Instance.Logger.Exception(e);
					}
					finally
					{
						G.Sys.MenuPanelManager_.Pop();
						__instance.levelGridMenu_.CreateEntries();
					}
				})
				.Show();
			}
		}
	}
}
