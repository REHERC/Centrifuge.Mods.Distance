using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Distance.NoEditorNumberLimits.Harmony
{
	[HarmonyPatch(typeof(TransformWrapper), "Visit")]
	internal static class TransformWrapper__Visit
	{
		// Array to match original method's positive forcing
		// vector._ = Mathf.Max(Mathf.Abs(vector._), 1E-05f);
		internal static readonly OpCode[] Match = new OpCode[]
		{
			OpCodes.Ldloca_S,
			OpCodes.Ldloca_S,
			OpCodes.Ldfld,
			OpCodes.Call,
			OpCodes.Ldc_R4,
			OpCodes.Call,
			OpCodes.Stfld,
		};

		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
		{
			List<CodeInstruction> code = new List<CodeInstruction>(instr);

			for (int index = 0; index < instr.Count(); index++)
			{
				if (index < instr.Count() - Match.Length)
				{
					var next = instr.Skip(index).Take(Match.Length).Select((x) => x.opcode);

					if (next.SequenceEqual(Match))
					{
						for (int i = 0; i < Match.Length; i++)
						{
							code[index + i].opcode = OpCodes.Nop;
						}

						index += Match.Length - 1;
					}
				}
			}

			return code.ToArray();
		}
	}
}
