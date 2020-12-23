using Distance.TrackMusic.Models;
using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(NGUIComponentInspector), "Init")]
	internal static class NGUIComponentInspector__Init
	{
		[HarmonyPostfix]
		internal static void Postfix(NGUIComponentInspector __instance)
		{
			if (__instance.ISerializable_ != null && __instance.ISerializable_.GetType() == typeof(ZEventListener))
			{
				if (((ZEventListener)__instance.ISerializable_).eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicChoice>()))
				{
					__instance.SetRemoveComponentButtonVisibility(false);
				}
				else if (((ZEventListener)__instance.ISerializable_).eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicTrack>()))
				{
					__instance.SetRemoveComponentButtonVisibility(false);
				}
			}
		}
	}
}
