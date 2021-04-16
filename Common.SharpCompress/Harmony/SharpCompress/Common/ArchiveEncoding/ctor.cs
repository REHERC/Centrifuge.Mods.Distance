using HarmonyLib;
using SharpCompress.Common;
using System.Text;

namespace Common.SharpCompress.Harmony
{
	/* This patch is required for the following reasons:
	 * When running distance, the game runs with modo which is basically .Net 3.5
	 * The thing is that the default encoding for the archives is a special one (CodePage 437)
	 * And this special encoding isn't present in mono
	 * Basically this patch avoids crashes
	 * 
	 * To be sure files are both written and read the same way,
	 * the patch is also applied to the tool itself
	 * 
	 * See this link for more info https://gist.github.com/REHERC/e6ac069bf52659a02b6042604d4c166d
	 */
	[HarmonyPatch(typeof(ArchiveEncoding), MethodType.Constructor)]
	internal static class SharpCompress__Common__ArchiveEncoding___ctor__Patch
	{
		[HarmonyPrefix]
		internal static bool Prefix(ArchiveEncoding __instance)
		{
			// Use UTF-8 so 7-zip can read the arcvhive (unicode is unsupported)
			__instance.Default = __instance.Password = Encoding.UTF8;
			return false;
		}
	}
}
