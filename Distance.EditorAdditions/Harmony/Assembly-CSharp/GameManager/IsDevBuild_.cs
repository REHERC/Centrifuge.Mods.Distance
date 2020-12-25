using HarmonyLib;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(GameManager), "IsDevBuild_", MethodType.Getter)]
	internal static class GameManager__IsDevBuild_get
	{
		internal static bool Prefix(ref bool value)
		{
			if (Mod.DevMode)
			{
				value = true;
				return false;
			}

			return true;
		}
	}
}
