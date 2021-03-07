using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(LevelEditorMusicTrackSelectMenuLogic), "GenerateMusicNameList")]
	internal static class LevelEditorMusicTrackSelectMenuLogic__GenerateMusicNameList
	{
		[HarmonyPostfix]
		internal static void Postfix(LevelEditorMusicTrackSelectMenuLogic __instance)
		{
			if (Mod.Instance.Config.AdvancedMusicSelection && !G.Sys.GameManager_.IsDevBuild_)
			{
				__instance.buttonList_.Clear();
				List<AudioManager.MusicCue> music = G.Sys.AudioManager_.MusicCues_;

				if (!Mod.Instance.Config.AdvancedMusicSelection)
				{
					music.RemoveAll(x => x.devEvent_);
				}

				__instance.CreateButtons(music, Color.white);
				__instance.buttonList_.SortAndUpdateVisibleButtons();
			}
		}
	}
}
