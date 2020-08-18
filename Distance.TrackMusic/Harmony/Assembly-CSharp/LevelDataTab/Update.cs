using HarmonyLib;
using System;
using UnityEngine;

namespace Distance.TrackMusic.Harmony
{
    [HarmonyPatch(typeof(LevelDataTab), "Update")]
    internal static class LevelDataTab__Update
    {
        [HarmonyPostfix]
        internal static void Postfix(LevelDataTab __instance, ref bool ___propertiesAreBeingDisplayed_)
        {
            Mod mod = Mod.Instance;

            if (___propertiesAreBeingDisplayed_ || __instance.IsSelectionValid_)
            {
                if (mod.LevelEditor.NeedsRefresh)
                {
                    mod.LevelEditor.NeedsRefresh = false;

                    try
                    {
                        __instance.GetComponent<NGUIObjectInspectorTabAbstract>()?.ClearComponentInspectors();
                        __instance.GetComponent<NGUIObjectInspectorTab>()?.InitComponentInspectors();
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Failed to refresh LevelDataTab: {e}");
                    }
                }
            }
        }
    }
}
