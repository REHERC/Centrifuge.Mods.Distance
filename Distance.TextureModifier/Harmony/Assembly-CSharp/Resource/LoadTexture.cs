using Harmony;
using UnityEngine;

namespace Distance.TextureModifier.Harmony
{
    [HarmonyPatch(typeof(Resource), "LoadTexture")]
    internal static class Resource__LoadTexture
    {
        [HarmonyPrefix]
        internal static void Prefix(ref Texture __result)
        {
            __result = Entry.Instance.Loader.GetRandomTexture();
        }
    }
}
