using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace CustomCar
{
    internal static class CustomCarsPatchInfos
    {
        public const int baseCarCount = 6;
        public static int carCount = 0;
    }

    [HarmonyPatch(typeof(Profile), "Awake")]
    internal class ProfileAwake
    {
        private static void Postfix(Profile __instance)
        {
            var carColors = new CarColors[G.Sys.ProfileManager_.carInfos_.Length];
            for (var i = 0; i < carColors.Length; i++)
                carColors[i] = G.Sys.ProfileManager_.carInfos_[i].colors_;

            var field = __instance.GetType().GetField("carColorsList_", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(__instance, carColors);
        }
    }

    [HarmonyPatch(typeof(Profile), "SetColorsForAllCars")]
    internal class ProfileSetColorsForAllCars
    {
        private static bool Prefix(Profile __instance, CarColors cc)
        {
            var carColors = new CarColors[G.Sys.ProfileManager_.carInfos_.Length];
            for (var i = 0; i < carColors.Length; i++)
                carColors[i] = cc;

            var field = __instance.GetType().GetField("carColorsList_", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(__instance, carColors);

            field = __instance.GetType().GetField("dataModified_", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(__instance, true);

            return false;
        }
    }

    [HarmonyPatch(typeof(Profile), "Save")]
    internal class ProfileSave
    {
        private static void Postfix(Profile __instance)
        {
            ModdedCarsColors.SaveAll();
        }
    }

    //change additive to blend animation blendMode
    [HarmonyPatch(typeof(GadgetWithAnimation), "SetAnimationStateValues")]
    internal class GadgetWithAnimationSetAnimationStateValues
    {
        private static bool Prefix(GadgetWithAnimation __instance)
        {
            var comp = __instance.GetComponentInChildren<Animation>();
            if (comp)
            {
                if (!ChangeBlendModeToBlend(comp.transform, __instance.animationName_))
                    return true;

                var state = comp[__instance.animationName_];
                if (state)
                {
                    state.layer = 3;
                    state.blendMode = AnimationBlendMode.Blend;
                    state.wrapMode = WrapMode.ClampForever;
                    state.enabled = true;
                    state.weight = 1f;
                    state.speed = 0f;
                }
            }

            return false;
        }

        private static bool ChangeBlendModeToBlend(Transform obj, string animationName)
        {
            for (var i = 0; i < obj.childCount; i++)
            {
                var n = obj.GetChild(i).gameObject.name.ToLower();
                if (!n.StartsWith("#"))
                    continue;

                n = n.Remove(0, 1);
                var parts = n.Split(';');

                if (parts.Length == 1)
                {
                    if (parts[0] == "additive")
                        return false;
                    if (parts[0] == "blend")
                        return true;
                }

                if (parts[1] == animationName.ToLower())
                {
                    if (parts[0] == "additive")
                        return false;
                    if (parts[0] == "blend")
                        return true;
                }
            }

            return false;
        }
    }

    //[HarmonyPatch(typeof(HornGadget), "OnCarHornEvent")]
    //internal class HornGadgetOnCarHornEvent
    //{
    //    static void Postfix(HornGadget __instance, Horn.Data data)
    //    {
    //        LogCarPrefabs.LogObjectAndChilds(__instance.gameObject);
    //    }
    //}
}