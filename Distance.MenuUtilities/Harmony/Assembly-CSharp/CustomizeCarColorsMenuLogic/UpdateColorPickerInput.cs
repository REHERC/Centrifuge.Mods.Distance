using Distance.MenuUtilities.Scripts;
using HarmonyLib;

namespace Distance.MenuUtilities.Harmony.Assembly_CSharp
{
	[HarmonyPatch(typeof(CustomizeCarColorsMenuLogic), "UpdateColorPickerInput")]
	internal class CustomizeCarColorsMenuLogic__UpdateColorPickerInput
	{
		[HarmonyPrefix]
		internal static bool Prefix(CustomizeCarColorsMenuLogic __instance)
		{
			CustomizeMenuCompoundData data = __instance.GetComponent<CustomizeMenuCompoundData>();

			if (data && data.IsEditing)
			{
				return false;
			}

			return true;
		}
	}
}
