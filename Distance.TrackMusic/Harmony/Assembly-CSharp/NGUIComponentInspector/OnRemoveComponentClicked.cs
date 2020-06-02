using Distance.TrackMusic.Models;
using Harmony;
using System.Linq;
using UnityEngine;

namespace Distance.TrackMusic.Harmony
{
    [HarmonyPatch(typeof(NGUIComponentInspector), "OnRemoveComponentClicked")]
    internal static class NGUIComponentInspector__OnRemoveComponentClicked
    {
        [HarmonyPrefix]
        internal static bool Prefix(NGUIComponentInspector __instance)
        {
            if (__instance.ISerializable_ is ZEventListener && ((ZEventListener)__instance.ISerializable_).eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicChoice>()))
            {
                EditorTools.RemoveMusicChoiceTool removeTool = G.Sys.LevelEditor_.StartNewToolJobOfType(typeof(EditorTools.RemoveMusicChoiceTool), false) as EditorTools.RemoveMusicChoiceTool;
                
                if (removeTool != null)
                {
                    var ser = __instance.iSerializables_;
                    removeTool.SetComponents(ser.Cast<Component>().ToArray());
                }
                return false;
            }
            return true;
        }
    }
}
