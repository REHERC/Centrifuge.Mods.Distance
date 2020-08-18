using HarmonyLib;

namespace Distance.CustomWheelHologram.Harmony
{
    [HarmonyPatch(typeof(WheelPOV), "LateUpdate")]
    internal class WheelPOV__LateUpdate
    {
        [HarmonyPostfix]
        internal static void Postfix(WheelPOV __instance)
        {
            __instance.renderer_.material.mainTexture = Mod.Instance.Config.Enabled ? Mod.Instance.CustomImage : Mod.Instance.OriginalImage ?? null;
        }
    }
}
