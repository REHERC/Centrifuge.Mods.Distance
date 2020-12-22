using HarmonyLib;
using UnityEngine;

namespace Distance.CustomCar.Harmony
{
    [HarmonyPatch(typeof(GadgetWithAnimation), "SetAnimationStateValues")]
	internal class GadgetWithAnimation__SetAnimationStateValues
	{
		[HarmonyPrefix]
        internal static bool Prefix(GadgetWithAnimation __instance)
        {
            Animation animation = __instance.GetComponentInChildren<Animation>(true);
            if (animation)
            {
                return PatchAnimations(animation, __instance.animationName_);
            }

            return false;
        }

        private static bool PatchAnimations(Animation animation, string name)
		{
            if (animation)
			{
                if (!ChangeBlendModeToBlend(animation.transform, name))
                {
                    return true;
                }


                AnimationState state = animation[name];
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
            for (int childIndex = 0; childIndex < obj.childCount; childIndex++)
            {
                string name = obj.GetChild(childIndex).gameObject.name.ToLower();
                if (!name.StartsWith("#"))
                {
                    continue;
                }

                name = name.Remove(0, 1);
                string[] parts = name.Split(';');

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
}
