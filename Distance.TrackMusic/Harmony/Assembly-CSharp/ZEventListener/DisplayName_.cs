using Distance.TrackMusic.Models;
using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(ZEventListener), "DisplayName_", MethodType.Getter)]
	internal static class ZEventListener__DisplayName_getter
	{
		[HarmonyPrefix]
		internal static bool Prefix(ZEventListener __instance, ref string __result)
		{
			if (__instance == null)
			{
				return true;
			}

			if (__instance.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicChoice>()))
			{
				__result = "Music Choice";
				return false;
			}
			else if (__instance.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicTrack>()))
			{
				__result = "Music Track";
				return false;
			}

			return true;
		}
	}
}
