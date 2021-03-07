#pragma warning disable RCS1110
using HarmonyLib;
using System.Reflection;

public static class RuntimePatcher
{
	public static readonly Harmony HarmonyInstance;

	static RuntimePatcher()
	{
		HarmonyInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Assembly.GetExecutingAssembly().GetName().FullName);
	}

	public static void AutoPatch()
	{
		HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
	}
}