using Events.Local;
using HarmonyLib;

namespace Distance.CustomDeathMessages.Harmony
{
	[HarmonyPatch(typeof(ClientLogic), "OnEventToAllClientsRemotePlayerActionMessage")]
	internal class ClientLogic__OnEventToAllClientsRemotePlayerActionMessage
	{
		[HarmonyPrefix]
		internal static bool Prefix(ClientLogic __instance, ToAllClientsRemotePlayerActionMessage.Data data)
		{
			string message = data.message_;
			string username = __instance.ClientPlayerList_[data.index_].username_;
			string formattedName = __instance.ClientPlayerList_[data.index_].GetChatName(true);

			string replacement = Message.GetMessage(message, username, formattedName);

			Message.Send(replacement);

			return false;
		}
	}
}
