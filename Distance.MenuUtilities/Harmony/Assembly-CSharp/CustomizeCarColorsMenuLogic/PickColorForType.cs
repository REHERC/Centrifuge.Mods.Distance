using Distance.MenuUtilities.Scripts;
using HarmonyLib;

namespace Distance.MenuUtilities.Harmony
{
	[HarmonyPatch(typeof(CustomizeCarColorsMenuLogic), "PickColorForType")]
	internal static class CustomizeCarColorsMenuLogic__PickColorForType
	{
		[HarmonyPostfix]
		internal static void Postfix(CustomizeCarColorsMenuLogic __instance, ColorChanger.ColorType colorType)
		{
			CustomizeMenuCompoundData data = __instance.GetComponent<CustomizeMenuCompoundData>();

			if (data && Mod.Instance.Config.EnableHexColorInput)
			{
				data.ColorType = colorType;

				__instance.SetThirdActionEnabled(true);
				__instance.SetThirdAction("EDIT", InternalResources.Constants.INPUT_EDIT_COLOR, data.EditHexClick);
			}
		}
	}
}
