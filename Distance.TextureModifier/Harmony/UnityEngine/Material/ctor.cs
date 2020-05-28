using Harmony;
using System;
using UnityEngine;

namespace Distance.TextureModifier.Harmony
{
    //[HarmonyPatch(typeof(Material), new Type[1] { typeof(string) })]
    //[HarmonyPatch(typeof(Material), new Type[1] { typeof(Material) })]
    //[HarmonyPatch(typeof(Material), new Type[1] { typeof(Shader) })]
    internal static class UnityEngine__Material__AnyConstructor
    {
        [HarmonyPostfix]
        internal static void Postfix(Material __instance)
        {
            Entry.Instance.Modifier.Patch(__instance);
        }
    }
}
