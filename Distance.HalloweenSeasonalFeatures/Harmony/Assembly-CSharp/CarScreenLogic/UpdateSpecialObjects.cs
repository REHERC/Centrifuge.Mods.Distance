using HarmonyLib;
using UnityEngine;

namespace Distance.HalloweenSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(CarScreenLogic), "UpdateSpecialObjects")]
	internal static class CarScreenLogic__UpdateSpecialObjects
	{
		[HarmonyPostfix]
		internal static void Postfix(CarScreenLogic __instance)
		{
			Transform cobwebTransform = __instance.transform.Find("CircleParent/HalloweenCobwebPlatform");

			if (cobwebTransform)
			{
				cobwebTransform.gameObject.SetActive(true);
			}
		}
	}
}
