using HarmonyLib;
using LevelEditorTools;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(GenerateTrackmogrifyLevelTool), "Finish")]
	internal static class GenerateTrackmogrifyLevelTool__Finish
	{
		[HarmonyPrefix]
		internal static void Prefix()
		{
			EditorUtil.ClearQuickMemory();
		}
	}
}
