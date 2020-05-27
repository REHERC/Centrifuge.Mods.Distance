using Events.Managers;
using Harmony;

namespace Distance.Shared.Harmony
{
    [HarmonyPatch(typeof(GameManager), "Awake")]
    internal static class GameManager__Awake
    {
        [HarmonyPostfix]
        internal static void Postfix(GameManager __instance)
        {
            AwakeGameManager.Broadcast(new AwakeGameManager.Data(__instance));
        }
    }
}
