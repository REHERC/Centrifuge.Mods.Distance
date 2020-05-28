using Harmony;
using UnityEngine;

namespace Distance.TextureModifier.Harmony
{
    //[HarmonyPatch(typeof(Resource), "LoadPrefab")]
    internal static class Resource__LoadPrefabInstance
    {
        [HarmonyPostfix]
        internal static void Postfix(ref GameObject __result)
        {
            Entry.Instance.Modifier.Patch(__result);
        }
    }
}
