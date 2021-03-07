using HarmonyLib;
using SharpCompress.Common;
using System.Text;

namespace Common.SharpCompress.Harmony
{
	[HarmonyPatch(typeof(ArchiveEncoding), MethodType.Constructor)]
	internal static class SharpCompress__Common__ArchiveEncoding___ctor__Patch
	{
		[HarmonyPrefix]
		internal static bool Prefix(ArchiveEncoding __instance)
		{
			__instance.Default = __instance.Password = Encoding.Unicode;

			return false;
		}
	}
}
