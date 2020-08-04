using Events.Local;
using Harmony;

namespace Distance.CustomDeathMessages.Harmony
{
    [HarmonyPatch(typeof(ClientLogic), "OnEventToAllClientsRemotePlayerActionMessage")]
    internal class ClientLogic__OnEventToAllClientsRemotePlayerActionMessage
    {
        [HarmonyPrefix]
        internal static bool Prefix(ClientLogic __instance, ToAllClientsRemotePlayerActionMessage.Data data)
        {
            Mod.Instance.Logger.Warning("ClientLogic__OnEventToAllClientsRemotePlayerActionMessage");

            string message = data.message_;
            string username = __instance.ClientPlayerList_[data.index_].Username_;

            string replacement = Message.GetMessage(message, username);

            Message.Send(replacement);

            return false;
        }
    }
}
