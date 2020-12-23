using Distance.TrackMusic.Editor.Tools;
using Distance.TrackMusic.Models;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(NGUIComponentInspector), "OnRemoveComponentClicked")]
	internal static class NGUIComponentInspector__OnRemoveComponentClicked
	{
		[HarmonyPrefix]
		internal static bool Prefix(NGUIComponentInspector __instance)
		{
			if (__instance.ISerializable_ is ZEventListener listener && listener.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicChoice>()))
			{
				if (G.Sys.LevelEditor_.StartNewToolJobOfType(typeof(RemoveMusicChoiceTool), false) is RemoveMusicChoiceTool removeTool)
				{
					var ser = __instance.iSerializables_;
					removeTool.SetComponents(ser.Cast<Component>().ToArray());
				}

				return false;
			}

			return true;
		}
	}
}
