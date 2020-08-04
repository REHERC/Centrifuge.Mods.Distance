using Events.Local;
using Harmony;

namespace Distance.CustomDeathMessages.Harmony
{
    [HarmonyPatch(typeof(ClientLogic), "OnEventPlayerActionMessage")]
    internal class ClientLogic__OnEventPlayerActionMessage
    {
        [HarmonyPrefix]
        internal static bool Prefix(ClientLogic __instance, PlayerActionMessage.Data data)
        {
            Mod.Instance.Logger.Warning("ClientLogic__OnEventPlayerActionMessage");

            string message = data.message_;
            string username = __instance.GetLocalChatName();

            string replacement = Message.GetMessage(message, username);

            Message.Send(replacement);

            return false;
        }
    }
}
