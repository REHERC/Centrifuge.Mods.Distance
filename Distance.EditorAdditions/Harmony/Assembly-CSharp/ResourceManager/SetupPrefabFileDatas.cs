using HarmonyLib;
using Serializers;
using System.IO;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(ResourceManager), "SetupPrefabFileDatas")]
	internal static class ResourceManger__SetupPrefabFileDatas
	{
		[HarmonyPrefix]
		public static bool Prefix(ResourceManager __instance)
		{
			__instance.levelPrefabFileInfosRoot_ = new LevelPrefabFileInfo("Default", null);

			DirectoryInfo CustomObjects = new DirectoryInfo(Resource.PersonalCustomObjectsDirPath_);

			if (CustomObjects.Exists)
			{
				__instance.customObjectFileInfosRoot_ = new LevelPrefabFileInfo("Custom", __instance.levelPrefabFileInfosRoot_);
				__instance.levelPrefabFileInfosRoot_.AddChildInfo(__instance.customObjectFileInfosRoot_);

				AddSubfoldersRecursive(CustomObjects, __instance.customObjectFileInfosRoot_);
			}

			string path = Application.dataPath + "/Resources/LevelEditorPrefabDirectoryInfo.xml";
			XmlDeserializer xmlDeserializer = new XmlDeserializer(path);

			while (xmlDeserializer.Read("LevelEditorPrefabDirectoryInformation"))
			{
				__instance.ReadFileDataRecursive(xmlDeserializer, __instance.levelPrefabFileInfosRoot_);
			}

			xmlDeserializer?.Finish();

			return false;
		}

		public static void AddSubfoldersRecursive(DirectoryInfo directory, LevelPrefabFileInfo parent)
		{
			foreach (DirectoryInfo subdirectory in directory.GetDirectories())
			{
				AddSubfoldersRecursive(subdirectory, CreateSubdirectory(parent, subdirectory.Name));
			}

			foreach (FileInfo prefab in directory.GetFiles("*.bytes"))
			{
				AddOrUpdatePrefabInfo(parent, prefab);
			}
		}

		public static LevelPrefabFileInfo CreateSubdirectory(LevelPrefabFileInfo parent, string name)
		{
			LevelPrefabFileInfo child = new LevelPrefabFileInfo(name, parent);
			parent.AddChildInfo(child);

			return child;
		}

		public static LevelPrefabFileInfo AddOrUpdatePrefabInfo(LevelPrefabFileInfo parent, FileInfo file)
		{
			string prefab_path = Resource.NormalizePath(file.FullName);
			string name = Path.GetFileNameWithoutExtension(prefab_path);
			LevelPrefabFileInfo entry = parent.GetChildFileInfo((LevelPrefabFileInfo value) => value.Name_ == name);

			if (entry is null)
			{
				entry = new LevelPrefabFileInfo(name, prefab_path, parent);
				parent.AddChildInfo(entry);
			}
			else
			{
				entry.CustomPrefabPath_ = prefab_path;
			}

			return entry;
		}
	}
}
