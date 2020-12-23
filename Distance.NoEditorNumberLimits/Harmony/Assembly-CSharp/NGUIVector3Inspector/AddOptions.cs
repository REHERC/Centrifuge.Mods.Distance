using HarmonyLib;
using System.Collections.Generic;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(NGUIVector3Inspector), "AddOptions")]
	internal static class NGUIVector3Inspector__AddOptions
	{
		[HarmonyPostfix]
		internal static void Postfix(NGUIVector3Inspector __instance)
		{
			List<UIExNumericInput> inputs = new List<UIExNumericInput>(){
				__instance.inputX_,
				__instance.inputY_,
				__instance.inputZ_
			};

			foreach (var input in inputs)
			{
				input.Min_ = float.MinValue;
				input.Max_ = float.MaxValue;
			}
		}
	}
}
