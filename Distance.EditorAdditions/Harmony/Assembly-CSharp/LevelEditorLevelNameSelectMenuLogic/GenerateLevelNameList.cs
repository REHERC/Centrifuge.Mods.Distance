using HarmonyLib;
using System.Linq;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(LevelEditorLevelNameSelectMenuLogic), "GenerateLevelNameList")]
	internal class LevelEditorLevelNameSelectMenuLogic__GenerateLevelNameList
	{
		[HarmonyPostfix]
		internal static void Postfix(LevelEditorLevelNameSelectMenuLogic __instance)
		{
			if (Mod.Instance.Config.DisplayWorkshopLevels)
			{
				LevelSetsManager levelSets = G.Sys.LevelSets_;

				__instance.CreateButtons(levelSets.LevelsLevelFilePaths_.ToList<string>(), Colors.YellowColors.gold, LevelEditorLevelNameSelectMenuLogic.LevelPathEntry.DisplayOption.RelativePath);
				__instance.CreateButtons(levelSets.WorkshopLevelFilePaths_.ToList<string>(), GConstants.communityLevelColor_, LevelEditorLevelNameSelectMenuLogic.LevelPathEntry.DisplayOption.LevelName);

				__instance.buttonList_.SortAndUpdateVisibleButtons();
			}
		}
	}
}
