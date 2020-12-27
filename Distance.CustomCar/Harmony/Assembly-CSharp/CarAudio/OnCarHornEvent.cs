using Events.Car;
using HarmonyLib;
using UnityEngine;

namespace Distance.CustomCar.Harmony
{
	[HarmonyPatch(typeof(CarAudio), "OnCarHornEvent")]
	internal static class OnCarHornEvent
	{
		[HarmonyPrefix]
		internal static bool Prefix(CarAudio __instance, Horn.Data data)
		{
			int carIndex = G.Sys.ProfileManager_.knownCars_[__instance.carLogic_.PlayerData_.CarName_];
			
			if (carIndex >= Mod.DefaultCarCount && Mod.Instance.Config.UseTrumpetHorn)
			{


				__instance.phantom_.SetRTPCValue("Horn_volume", Mathf.Clamp01(data.hornPercent_ + 0.5f));
				__instance.phantom_.Play("SpookyHorn", getEvent: true);

				return false;
			}

			return true;
		}
	}
}
