using HarmonyLib;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(LibraryTab), "Start")]
	internal static class LibraryTab__Start
	{
		[HarmonyPrefix]
		internal static bool Prefix(LibraryTab __instance)
		{
			ConfigurationLogic Config = Mod.Instance.Config;

			__instance.iconSizeSlider_.onChange.Add(new EventDelegate(() => Config.EditorIconSize = __instance.IconSize_));

			__instance.iconSize_ = Config.EditorIconSize;

			__instance.rootFileData_ = G.Sys.ResourceManager_.LevelPrefabFileInfosRoot_;

			if (!Config.DevFolderEnabled && !G.Sys.GameManager_.IsDevBuild_)
			{
				__instance.rootFileData_.RemoveAllChildInfos((LevelPrefabFileInfo x) => x.IsDirectory_ && x.Name_ == "Dev");
			}

			__instance.currentDirectory_ = __instance.rootFileData_;
			__instance.iconSizeSlider_.value = Mathf.InverseLerp(32f, 256f, __instance.iconSize_);
			__instance.searchInput_ = __instance.GetComponentInChildren<UIExInput>();
			__instance.StartCoroutine(__instance.CreateIconsAfterAFrame());

			return false;
		}
	}
}
