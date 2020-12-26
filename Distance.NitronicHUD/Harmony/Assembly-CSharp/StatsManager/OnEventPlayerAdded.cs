using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace Distance.NitronicHUD.Harmony
{
	[HarmonyPatch(typeof(StatsManager), "OnEventPlayerAdded")]
	internal static class StatsManager__OnEventPlayerAdded
	{
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
		{
			return instr.Skip(5); // Skip the default check "if is in level editor, don't create player stats object"
		}
	}
}
