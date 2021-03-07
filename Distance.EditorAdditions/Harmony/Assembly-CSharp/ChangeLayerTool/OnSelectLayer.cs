using HarmonyLib;
using LevelEditorTools;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(ChangeLayerTool), "OnSelectLayer")]
	internal static class ChangeLayerTool__OnSelectLayer
	{
		[HarmonyPrefix]
		internal static bool Prefix(ChangeLayerTool __instance)
		{
			if (!EditorUtil.IsSelectionRoot())
			{
				EditorUtil.PrintToolInspectionStackError();
				__instance.state_ = ToolState.Cancelled;

				return false;
			}

			return true;
		}
	}
}
