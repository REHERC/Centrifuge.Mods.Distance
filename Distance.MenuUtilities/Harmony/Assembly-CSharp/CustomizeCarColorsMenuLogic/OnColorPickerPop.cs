using Distance.MenuUtilities.Scripts;
using HarmonyLib;

namespace Distance.MenuUtilities.Harmony
{
	[HarmonyPatch(typeof(CustomizeCarColorsMenuLogic), "OnColorPickerPop")]
	internal static class CustomizeCarColorsMenuLogic__OnColorPickerPop
	{
		[HarmonyPostfix]
		internal static void Postfix(CustomizeCarColorsMenuLogic __instance)
		{
			CustomizeMenuCompoundData data = __instance.GetComponent<CustomizeMenuCompoundData>();

			if (data)
			{
				data.ColorType = ColorChanger.ColorType.Size_;

				__instance.SetThirdActionEnabled(false);
				__instance.SetThirdAction(string.Empty, InputAction.MenuDoNotUse, null);
			}
		}
	}
}
