using Distance.TrackMusic.Models;
using HarmonyLib;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(ZEventListener), "Visit")]
	internal static class ZEventListener__Visit___MusicChoice
	{
		[HarmonyPrefix]
		internal static bool Prefix(ZEventListener __instance, IVisitor visitor)
		{
			Mod mod = Mod.Instance;
			if (!(visitor is NGUIComponentInspector))
			{
				return true;
			}

			NGUIComponentInspector inspector = visitor as NGUIComponentInspector;

			if (!__instance.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicChoice>()))
			{
				return true;
			}

			visitor.Visit("eventName_", ref __instance.eventName_, false, null);
			visitor.Visit("delay_", ref __instance.delay_, false, null);

			var isEditing = inspector.isEditing_;

			var data = mod.Variables.CachedMusicChoice.GetOrCreate(__instance, () => new MusicChoice());
			if (data.LastWrittenData != __instance.eventName_)
			{
				data.ReadObject(__instance);
				data.LastWrittenData = __instance.eventName_;
				data.LastWritten = data.Clone();
			}
			else if (!isEditing)
			{
				var anyChanges = false;
				var old = data.LastWritten;

				if (data.Choices.Count != old.Choices.Count)
				{
					anyChanges = true;
				}

				foreach (var newChoice in data.Choices)
				{
					if (!old.Choices.ContainsKey(newChoice.Key) || old.Choices[newChoice.Key].Track != newChoice.Value.Track)
					{
						anyChanges = true;
						break;
					}
				}
				if (anyChanges)
				{
					data.WriteObject(__instance);
					data.LastWrittenData = __instance.eventName_;
					data.LastWritten = data.Clone();
				}
			}

			for (int index = 0; index < data.Choices.Count; index++)
			{
				string key = data.Choices.Keys.ToArray()[index];
				var track = data.Choices[key].Track;

				visitor.Visit($"{key} Track", ref track, null);
			}

			return false;
		}

		[HarmonyPostfix]
		internal static void Postfix(ZEventListener __instance)
		{
			if (!__instance.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicChoice>()))
			{
				return;
			}

			Mod.Instance.Variables.CachedMusicChoice.GetOrCreate(__instance, () => MusicChoice.FromObject(__instance));
		}
	}
}
