using HarmonyLib;
using UnityEngine;

namespace Distance.CustomWheelHologram.Harmony
{
    [HarmonyPatch(typeof(WheelPOV), "Awake")]
    internal class WheelPOV__Awake
    {
        [HarmonyPostfix]
        internal static void Postfix(WheelPOV __instance)
        {
            if (!Mod.Instance.OriginalImage)
            {
                Mod.Instance.OriginalImage = __instance.renderer_.material.mainTexture as Texture2D;
            }
        }
    }
}
