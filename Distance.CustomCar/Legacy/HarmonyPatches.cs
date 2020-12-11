using HarmonyLib;
using UnityEngine;

namespace CustomCar.Legacy
{
    internal static class CustomCarsPatchInfos
    {
        public const int baseCarCount = 6;
        public static int carCount = 0;
    }

    [HarmonyPatch(typeof(Profile), "Awake")]
    internal class ProfileAwake
    {
        internal static void Postfix(Profile __instance)
        {
            CarColors[] carColors = new CarColors[G.Sys.ProfileManager_.carInfos_.Length];
            for (int i = 0; i < carColors.Length; i++)
            {
                carColors[i] = G.Sys.ProfileManager_.carInfos_[i].colors_;
            }

            __instance.carColorsList_ = carColors;
        }
    }

    [HarmonyPatch(typeof(Profile), "SetColorsForAllCars")]
    internal class ProfileSetColorsForAllCars
    {
        internal static bool Prefix(Profile __instance, CarColors cc)
        {
            CarColors[] carColors = new CarColors[G.Sys.ProfileManager_.carInfos_.Length];
            for (int i = 0; i < carColors.Length; i++)
            {
                carColors[i] = cc;
            }

            __instance.carColorsList_ = carColors;

            __instance.dataModified_ = true;

            return false;
        }
    }

    [HarmonyPatch(typeof(Profile), "Save")]
    internal class ProfileSave
    {
        internal static void Postfix()
        {
            ModdedCarsColors.SaveAll();
        }
    }

    //change additive to blend animation blendMode
    [HarmonyPatch(typeof(GadgetWithAnimation), "SetAnimationStateValues")]
    internal class GadgetWithAnimationSetAnimationStateValues
    {
        internal static bool Prefix(GadgetWithAnimation __instance)
        {
            Animation comp = __instance.GetComponentInChildren<Animation>();
            if (comp)
            {
                if (!ChangeBlendModeToBlend(comp.transform, __instance.animationName_))
                {
                    return true;
                }

                AnimationState state = comp[__instance.animationName_];
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
            for (int i = 0; i < obj.childCount; i++)
            {
                string n = obj.GetChild(i).gameObject.name.ToLower();
                if (!n.StartsWith("#"))
                {
                    continue;
                }

                n = n.Remove(0, 1);
                string[] parts = n.Split(';');

                if (parts.Length == 1)
                {
                    if (parts[0] == "additive")
                    {
                        return false;
                    }

                    if (parts[0] == "blend")
                    {
                        return true;
                    }
                }

                if (parts[1] == animationName.ToLower())
                {
                    if (parts[0] == "additive")
                    {
                        return false;
                    }

                    if (parts[0] == "blend")
                    {
                        return true;
                    }
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