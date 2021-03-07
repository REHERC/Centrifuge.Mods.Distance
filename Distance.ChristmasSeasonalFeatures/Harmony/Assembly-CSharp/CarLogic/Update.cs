using HarmonyLib;
using UnityEngine;

namespace Distance.ChristmasSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(CarLogic), "Update")]
	internal static class CarLogic__Update
	{
		[HarmonyPostfix]
		internal static void Postfix(CarLogic __instance)
		{
			Transform reindeerCosmetic = __instance.transform.Find(InternalResources.Constants.REINDEER_COSMETIC);

			if (reindeerCosmetic)
			{
				if (reindeerCosmetic.HasComponent<HolidayFeaturesToggle>())
				{
					reindeerCosmetic.GetComponent<HolidayFeaturesToggle>().Destroy();
				}

				bool flag = Mod.Instance.IsActive && Mod.Instance.Config.ReindeerCosmeticVisualCheat;
				reindeerCosmetic.gameObject.SetActive(flag);
			}
		}
	}
}
