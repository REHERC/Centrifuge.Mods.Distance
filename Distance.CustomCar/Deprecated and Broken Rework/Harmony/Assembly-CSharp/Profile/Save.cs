using HarmonyLib;

namespace Distance.CustomCar.Harmony
{
	[HarmonyPatch(typeof(Profile), "Save")]
	internal class Profile__Save
	{
		[HarmonyPostfix]
		internal static void Postfix(Profile __instance)
		{
			Mod.Instance.ProfileCarColors.SaveAll();
		}
	}
}
