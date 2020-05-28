using Harmony;
using UnityEngine;

namespace Distance.TextureModifier.Harmony
{
    //[HarmonyPatch(typeof(Resource), "LoadMaterial")]
    internal static class Resource__LoadMaterial
    {
        [HarmonyPostfix]
        internal static void Postfix(ref Material __result)
        {
            //__result = Entry.Instance.Loader.GetRandomMaterial();
            Entry.Instance.Modifier.Patch(__result);
        }
    }
}
