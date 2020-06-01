using Harmony;
using UnityEngine;

namespace Distance.TextureModifier.Harmony
{
    [HarmonyPatch(typeof(Resource), "LoadLevelPreviewTexture")]
    internal static class Resource__LoadLevelPreviewTexture
    {
        [HarmonyPostfix]
        internal static void Postfix(ref Texture __result)
        {
            __result = Object.Instantiate(Mod.Instance.Loader.GetRandomTexture());
        }
    }
}
