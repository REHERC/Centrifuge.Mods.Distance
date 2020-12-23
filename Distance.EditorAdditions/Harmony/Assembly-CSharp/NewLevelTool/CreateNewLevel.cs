using HarmonyLib;
using LevelEditorTools;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(NewLevelTool), "CreateNewLevel")]
	internal static class NewLevelTool__CreateNewLevel
	{
		[HarmonyPrefix]
		internal static void Prefix()
		{
			EditorUtil.ClearQuickMemory();
		}
	}
}
