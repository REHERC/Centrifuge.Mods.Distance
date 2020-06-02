using Harmony;

namespace Distance.TrackMusic.Harmony
{
    [HarmonyPatch(typeof(Level), "ClearAndReset")]
    internal static class Level__ClearAndReset
    {
        [HarmonyPrefix]
        internal static void Prefix(Level __instance, bool destroyObjects)
        {
            if (destroyObjects && !Mod.Instance.levelEditor_.IsWorkingStateLevel)
            {
                Mod.Instance.levelEditor_.ResetLevelSettings(__instance.Settings_);
            }

            Mod.Instance.levelEditor_.IsWorkingStateLevel = false;
        }
    }
}
