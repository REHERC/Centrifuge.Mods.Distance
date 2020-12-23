using HarmonyLib;
using LevelEditorTools;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(LoadLevelTool), "Update")]
	internal static class LoadLevelTool__Update
	{
		[HarmonyPrefix]
		internal static void Prefix()
		{
			EditorUtil.ClearQuickMemory();
		}
	}
}
