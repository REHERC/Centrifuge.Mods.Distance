using HarmonyLib;
using UnityEngine;

namespace Distance.MenuUtilities.Harmony
{
    [HarmonyPatch(typeof(CustomizeCarColorsMenuLogic), "SetButtonMessage")]
    internal class CustomizeCarColorsMenuLogic__SetButtonMessage
    {
        [HarmonyPostfix]
        internal static void Postfix(CustomizeCarColorsMenuLogic __instance)
        {
            GameObject bottomRightText = __instance.cancelButton_.gameObject.Parent();

            /*foreach (GameObject children in bottomRightText.GetChildren())
            {
                children.SetActive(true);

                foreach (UIButton button in children.GetComponents<UIButton>())
                {
                    button.enabled = true;
                }
            }*/
        }
    }
}
