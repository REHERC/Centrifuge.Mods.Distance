using HarmonyLib;
using System;

namespace Distance.Anolytics.Harmony
{
	[HarmonyPatch(typeof(FileEx), "WriteAllText")]
	internal static class FileEx__WriteAllText
	{
		[HarmonyPrefix]
		internal static bool Prefix(string path)
		{
			return !string.Equals(path, "playtesting_data.txt", StringComparison.OrdinalIgnoreCase);
		}
	}
}
