using HarmonyLib;
using System;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(UIExColorPickerPopup), "Initialize")]
	internal static class UIExColorPickerPopup__Initialize
	{
		[HarmonyPrefix]
		internal static void Prefix()
		{
			//Mod.Instance.Logger.Info(Environment.StackTrace);
		}
	}
}
