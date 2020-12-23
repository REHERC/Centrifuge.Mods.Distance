using Distance.TrackMusic.Models;
using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(ZEventListener), "ComponentDescription_", MethodType.Getter)]
	internal static class ZEventListener__ComponentDescription_getter
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
				__result = "Custom music track choice";
				return false;
			}
			else if (__instance.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicTrack>()))
			{
				__result = "Custom music track data";
				return false;
			}

			return true;
		}
	}
}
