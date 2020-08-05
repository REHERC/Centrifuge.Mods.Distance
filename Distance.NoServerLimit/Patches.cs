using ExampleNamespace;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Distance.NoServerLimit
{
    internal static class Patches
    {
        [HarmonyPatch(typeof(NetworkingManager), "CreateServer")]
        public static class CreateServerPatch
        {
            public static bool Prefix(NetworkingManager __instance, string serverTitle, string password, int maxPlayerCount)
            {
                Network.InitializeSecurity();
                try
                {
                    __instance.password_ = password;
                    __instance.serverTitle_ = serverTitle;
                    G.Sys.GameData_.SetString("ServerTitleDefault", __instance.serverTitle_);
                    __instance.maxPlayerCount_ = Mathf.Clamp(maxPlayerCount, 1, Mod.Settings.GetItem<int>("MaxPlayerCount"));
                    G.Sys.GameData_.SetInt("MaxPlayersDefault", __instance.maxPlayerCount_);
                    int num = 1;
                    int connections = __instance.maxPlayerCount_ - num;
                    NetworkConnectionError networkConnectionError = Network.InitializeServer(connections, 32323, true);
                    if (networkConnectionError != NetworkConnectionError.NoError)
                    {
                        G.Sys.MenuPanelManager_.ShowError("Failed to create game lobby. Error code: " + networkConnectionError.ToString(), "Network Error", null, UIWidget.Pivot.Center);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }

                return false;
            }

            [HarmonyPatch(typeof(HostAGame), "IncrementMaxPlayers")]
            public static class IncrementMaxPlayersPatch
            {
                public static bool Prefix(HostAGame __instance, int direction)
                {
                    __instance.internalMaxPlayerCalc_ = GUtils.mod(__instance.internalMaxPlayerCalc_ + direction, Mod.Settings.GetItem<int>("MaxPlayerCount"));
                    __instance.maxPlayersLabel_.text = __instance.MaxPlayers_.ToString();
                    if (direction != 0 && AudioManager.Valid())
                    {
                        G.Sys.AudioManager_.PlaySound("ButtonSelect", "Menus", 1f);
                    }

                    return false;
                }
            }
        }
    }
}
