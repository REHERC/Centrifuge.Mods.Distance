using HarmonyLib;

namespace Distance.Anolytics.Harmony
{
	[HarmonyPatch(typeof(Analytics), "LogEvent")]
	internal static class Analytics__LogEvent
	{
		[HarmonyPrefix]
		internal static bool Prefix(string page, string category, string action, string opt_label, int opt_value)
		{
			Mod mod = Mod.Instance;

			if (mod.Config.LogAnalytics)
			{
				mod.Logger.Info($"Analytics request:\n  Page: {page}\n  Category: {category}\n  Action: {action}\n  Opt. Label: {opt_label}\n  Opt. Value: {opt_value}");
			}

			return !mod.Config.ShutDownAnalytics;
		}
	}
}
