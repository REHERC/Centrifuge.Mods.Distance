using Harmony;
using UnityEngine;

namespace Distance.TextureModifier.Harmony
{
    //[HarmonyPatch(typeof(Resource), "LoadMaterialInstance")]
    internal static class Resource__LoadMaterialInstance
    {
        [HarmonyPostfix]
        internal static void Postfix(ref Material __result)
        {
            Entry.Instance.Modifier.Patch(__result);
        }
    }
}
