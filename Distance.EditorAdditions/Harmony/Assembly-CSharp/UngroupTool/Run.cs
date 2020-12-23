using HarmonyLib;
using LevelEditorTools;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(UngroupTool), "Run")]
	internal static class UngroupTool__Run
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
