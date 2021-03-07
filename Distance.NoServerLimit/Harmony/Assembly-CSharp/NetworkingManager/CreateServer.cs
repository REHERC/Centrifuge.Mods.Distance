using HarmonyLib;
using System;
using UnityEngine;

namespace Distance.NoServerLimit.Harmony
{
	[HarmonyPatch(typeof(NetworkingManager), "CreateServer")]
	internal static class NetworkingManager__CreateServer
	{
		internal static bool Prefix(NetworkingManager __instance, string serverTitle, string password, int maxPlayerCount)
		{
			Network.InitializeSecurity();

			try
			{
				__instance.password_ = password;
				__instance.serverTitle_ = serverTitle;

				G.Sys.GameData_.SetString("ServerTitleDefault", __instance.serverTitle_);

				__instance.maxPlayerCount_ = Mathf.Clamp(maxPlayerCount, 1, Mod.Instance.Config.MaxPlayerCount);

				G.Sys.GameData_.SetInt("MaxPlayersDefault", __instance.maxPlayerCount_);

				const int num = 1;
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
				Mod.Instance.Logger.Exception(ex);
			}

			return false;
		}
	}
}
