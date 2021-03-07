using Events;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace Distance.HalloweenSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(HalloweenAudioLogic), "RandomGlitches")]
	internal static class HalloweenAudioLogic__RandomGlitches
	{
		[HarmonyPrefix]
		internal static bool Prefix(ref IEnumerator __result, HalloweenAudioLogic __instance)
		{
			__result = Enumerator(__instance);

			return false;
		}

		internal static IEnumerator Enumerator(HalloweenAudioLogic __instance)
		{
			yield return __instance.StartCoroutine(PatchCoroutine(__instance));
		}

		internal static IEnumerator PatchCoroutine(HalloweenAudioLogic __instance)
		{
			for (; ; )
			{
				float randomShortTime = Random.Range(0.1f, 1.5f);
				float randomLongTime = Random.Range(5f, 10f);

				if (__instance.vhs_)
				{
					__instance.vhs_.enabled = false;
				}

				if (__instance.glitch_)
				{
					__instance.glitch_.enabled = false;
				}

				AudioManager.PostEvent("Enable_Bypass_Darkness_Insane");

				if (__instance.pMan_ && __instance.pMan_.CurrentProfile_ && __instance.pMan_.CurrentProfile_.CarName_ == "Spectrum")
				{
					__instance.FlickerObjects(true, randomLongTime);
				}

				yield return new WaitForSeconds(randomLongTime);

				if (__instance.vhs_)
				{
					__instance.vhs_.enabled = true;
				}

				if (__instance.glitch_)
				{
					__instance.glitch_.enabled = true;
				}

				AudioManager.PostEvent("Disable_Bypass_Darkness_Insane");

				StaticEvent<VirusSpiritShake.Data>.Broadcast(new VirusSpiritShake.Data(0.075f));

				if (__instance.pMan_ && __instance.pMan_.CurrentProfile_ && __instance.pMan_.CurrentProfile_.CarName_ == "Spectrum")
				{
					__instance.FlickerObjects(false, randomShortTime);
				}

				yield return new WaitForSeconds(randomShortTime);
			}
		}
	}
}
