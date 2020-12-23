using Distance.TrackMusic.Models;
using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(GameObjectEx), "GetDisplayName")]
	internal static class GameObjectEx__GetDisplayName
	{
		[HarmonyPrefix]
		internal static bool Prefix(UnityEngine.GameObject gameObject, ref string __result)
		{
			if (gameObject == null)
			{
				return true;
			}

			var component = gameObject.GetComponent<ZEventListener>();

			if (component == null)
			{
				return true;
			}

			if (component.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicTrack>()))
			{
				var track = Mod.Instance.Variables.CachedMusicTrack.GetOrCreate(component, () => MusicTrack.FromObject(component));

				if (track == null)
				{
					__result = "Music Track?";
				}

				__result = $"Music Track: {track.Name}";

				return false;
			}
			return true;
		}
	}
}
