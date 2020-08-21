using Events.Local;
using HarmonyLib;

namespace Distance.CustomDeathMessages.Harmony
{
    [HarmonyPatch(typeof(ClientLogic), "OnEventPlayerActionMessage")]
    internal class ClientLogic__OnEventPlayerActionMessage
    {
        [HarmonyPrefix]
        internal static bool Prefix(ClientLogic __instance, PlayerActionMessage.Data data)
        {
            string message = data.message_;
            string username = __instance.GetLocalPlayerInfo().username_;
            string formattedName = __instance.GetLocalPlayerInfo().GetChatName(true);

            string replacement = Message.GetMessage(message, username, formattedName);

            Message.Send(replacement);

            return false;
        }
    }
}
