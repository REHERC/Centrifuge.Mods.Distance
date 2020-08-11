using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Distance.CustomDeathMessages
{
    public static class Message
    {
        internal const string ResetFormatting = "[-][-][-][-][-]";

        internal static readonly List<KeyValuePair<string, MessageType>> MessageDictionary = new List<KeyValuePair<string, MessageType>>()
        {
            new KeyValuePair<string, MessageType>("was terminated by the laser grid", MessageType.KillGrid ),
            new KeyValuePair<string, MessageType>("reset", MessageType.SelfTermination ),
            new KeyValuePair<string, MessageType>("was wrecked after getting split", MessageType.LaserOverheated ),
            new KeyValuePair<string, MessageType>("got wrecked?", MessageType.AntiTunnelSquish ),
            new KeyValuePair<string, MessageType>("got wrecked", MessageType.Impact ),
            new KeyValuePair<string, MessageType>("exploded from overheating", MessageType.Overheated ),
            new KeyValuePair<string, MessageType>("multiplier!", MessageType.KillGrid ),
            new KeyValuePair<string, MessageType>("was kicked due to not having this level", MessageType.KickNoLevel ),
            new KeyValuePair<string, MessageType>("finished", MessageType.Finished ),
            new KeyValuePair<string, MessageType>("is not ready", MessageType.NotReady ),
            new KeyValuePair<string, MessageType>("left the match to spectate", MessageType.Spectate ),
            new KeyValuePair<string, MessageType>("has taken the lead!", MessageType.TagPointsLead )
        };

        public static MessageType GetMessageType(string message)
        {
            foreach (var element in MessageDictionary)
            {
                if (message.ToLower().Contains(element.Key.ToLower()))
                {
                    return element.Value;
                }
            }

            return MessageType.None;
        }

        public static int GetStuntMultiplier(string message)
        {
            int.TryParse(message.Substring(14), out int result);
            return result;
        }

        public static string GetMessage(string message, string username)
        {
            MessageType type = GetMessageType(message);

            int stuntMultiplier = 0;

            if (type == MessageType.StuntCollect)
            {
                stuntMultiplier = GetStuntMultiplier(message);
            }

            JArray values = Mod.Instance.Config.Get(type.ToString(), new JArray());

            string[] entries = values.ToObject<string[]>();

            if (type == MessageType.None || !entries.Any())
            {
                return $"{username} {message}";
            }

            return string.Format(entries.RandomElement(), username, stuntMultiplier);
        }

        public static void Send(string message)
        {
            Events.ClientToAllClients.ChatMessage.Broadcast(new Events.ClientToAllClients.ChatMessage.Data(message));
        }
    }
}
