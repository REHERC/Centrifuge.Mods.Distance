using Harmony;

namespace Distance.Anolytics.Harmony
{
    [HarmonyPatch(typeof(Analytics), "LogEvent")]
    internal class Analytics__LogEvent
    {
        [HarmonyPrefix]
        internal static bool Prefix(string page, string category, string action, string opt_label, int opt_value)
        {
            Mod mod = Mod.Instance;

            if (mod.Config.LogAnalytics)
            {
                mod.Logger.Info($"Analytics request:\r\n  Page: {page}\r\n  Category: {category}\r\n  Action: {action}\r\n  Opt. Label: {opt_label}\r\n  Opt. Value: {opt_value}");
            }

            return !mod.Config.ShutDownAnalytics;
        }
    }
}
