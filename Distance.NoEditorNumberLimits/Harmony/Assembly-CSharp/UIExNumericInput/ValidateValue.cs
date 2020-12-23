using HarmonyLib;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(UIExNumericInput), "ValidateValue")]
	internal static class UIExNumericInput__ValidateValue
	{
		[HarmonyPrefix]
		internal static bool Prefix(ref float __result, float val)
		{
			if (float.IsNaN(val))
			{
				__result = 0;
			}
			else
			{
				__result = val;
			}

			return false;
		}
	}
}
