using Distance.MenuUtilities.Scripts;
using HarmonyLib;

namespace Distance.MenuUtilities.Harmony
{
    [HarmonyPatch(typeof(CustomizeCarColorsMenuLogic), "PickColorForType")]
    internal class CustomizeCarColorsMenuLogic__PickColorForType
    {
        [HarmonyPostfix]
        internal static void Postfix(CustomizeCarColorsMenuLogic __instance, ColorChanger.ColorType colorType)
        {
            CustomizeMenuCompoundData data = __instance.GetComponent<CustomizeMenuCompoundData>();

            if (data)
            {
                data.ColorType = colorType;

                __instance.SetThirdActionEnabled(true);
                __instance.SetThirdAction("EDIT", InternalResources.Constants.EDIT_COLOR_INPUT, data.EditHexClick);
            }
        }
    }
}
