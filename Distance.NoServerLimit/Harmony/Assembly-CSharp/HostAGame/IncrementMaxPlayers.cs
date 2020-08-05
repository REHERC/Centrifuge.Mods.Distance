using Harmony;

namespace Distance.NoServerLimit.Harmony
{
    [HarmonyPatch(typeof(HostAGame), "IncrementMaxPlayers")]
    internal class HostAGame__IncrementMaxPlayers
    {
        internal static bool Prefix(HostAGame __instance, int direction)
        {
            __instance.internalMaxPlayerCalc_ = GUtils.mod(__instance.internalMaxPlayerCalc_ + direction, Mod.Instance.Config.MaxPlayerCount);
            __instance.maxPlayersLabel_.text = __instance.MaxPlayers_.ToString();

            if (direction != 0 && AudioManager.Valid())
            {
                G.Sys.AudioManager_.PlaySound("ButtonSelect", "Menus", 1f);
            }

            return false;
        }
    }
}
