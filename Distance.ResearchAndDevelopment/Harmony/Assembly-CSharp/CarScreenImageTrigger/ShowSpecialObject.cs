using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace Distance.ResearchAndDevelopment.Harmony
{
	[HarmonyPatch(typeof(CarScreenImageTrigger), "ShowSpecialObject")]
	internal static class CarScreenImageTrigger__ShowSpecialObject
	{
		internal const string PREFIX = "@ScreenImage=";

		[HarmonyPrefix]
		internal static bool Prefix(CarScreenImageTrigger __instance, ref IEnumerator __result)
		{
			Mod.Instance.Logger.Info("ShowSpecialObject::Prefix");

			ZEventListener listener = __instance.GetComponent<ZEventListener>();

			if (listener && listener.eventName_.StartsWith(PREFIX)) {
				__result = ShowSpecialObject(__instance);
				return false;
			}

			return true;
		}

		internal static IEnumerator ShowSpecialObject(CarScreenImageTrigger __instance)
		{
			Mod.Instance.Logger.Info("ShowSpecialObject::Patch");

			ZEventListener listener = __instance.GetComponent<ZEventListener>();

			string data = listener.eventName_.Substring(PREFIX.Length);

			if (int.TryParse(data, out int value)) {
				CarScreenLogic screen = __instance.pd_.CarScreenLogic_;

				yield return new WaitForSeconds(__instance.delay_);

				screen.ShowSpecialObject(value, __instance.timeVisible_, __instance.showOnTriggerStay_);
			}
			else
			{
				Mod.Instance.Logger.Error($"Cannot parse \"{data}\" as a valid number");
			}
		}
	}
}
