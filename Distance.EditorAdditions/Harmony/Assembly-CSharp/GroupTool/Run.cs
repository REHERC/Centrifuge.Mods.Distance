using HarmonyLib;
using LevelEditorTools;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(GroupTool), "Run")]
	internal static class GroupTool__Run
	{
		[HarmonyPrefix]
		internal static bool Prefix(ref bool __result)
		{
			if (!EditorUtil.IsSelectionRoot())
			{
				EditorUtil.PrintToolInspectionStackError();
				__result = false;

				return false;
			}
			return true;
		}
	}
}
