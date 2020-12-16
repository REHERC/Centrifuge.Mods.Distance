using HarmonyLib;

namespace Distance.CustomCar.Harmony
{
	[HarmonyPatch(typeof(Profile), "SetColorsForAllCars")]
	internal class Profile__SetColorsForAllCars
	{
        [HarmonyPrefix]
        internal static bool Prefix(Profile __instance, CarColors cc)
        {
            CarColors[] carColors = new CarColors[G.Sys.ProfileManager_.carInfos_.Length];
            for (int i = 0; i < carColors.Length; i++)
            {
                carColors[i] = cc;
            }

            __instance.carColorsList_ = carColors;
            __instance.dataModified_ = true;

            return false;
        }
    }
}
