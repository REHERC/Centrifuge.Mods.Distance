﻿using Harmony;
using UnityEngine;

namespace Distance.TextureModifier.Harmony
{
    [HarmonyPatch(typeof(Resource), "LoadTextureFromFile")]
    internal static class Resource__LoadTextureFromFile
    {
        [HarmonyPostfix]
        internal static void Postfix(ref Texture __result)
        {
            __result = Object.Instantiate(Entry.Instance.Loader.GetRandomTexture());
        }
    }
}
