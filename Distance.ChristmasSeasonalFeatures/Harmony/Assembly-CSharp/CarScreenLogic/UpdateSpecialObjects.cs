using HarmonyLib;
using UnityEngine;

namespace Distance.ChristmasSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(CarScreenLogic), "UpdateSpecialObjects")]
	internal static class CarScreenLogic__UpdateSpecialObjects
	{
		[HarmonyPostfix]
		internal static void Postfix(CarScreenLogic __instance)
		{
			Transform snowTransform = __instance.transform.Find("CircleParent/ChristmasSnow");

			if (snowTransform)
			{
				if (snowTransform.HasComponent<HolidayFeaturesToggle>())
				{
					snowTransform.GetComponent<HolidayFeaturesToggle>().Destroy();
				}

				bool flag = Mod.Instance.IsActive && Mod.Instance.Config.ChristmasSnowVisualCheat;

				snowTransform.gameObject.SetActive(flag);
			}
		}
	}
}
