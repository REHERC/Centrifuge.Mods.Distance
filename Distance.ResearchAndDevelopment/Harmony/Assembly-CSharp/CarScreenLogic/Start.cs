using HarmonyLib;
using Reactor.API.Configuration;
using Reactor.API.Storage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Distance.ResearchAndDevelopment.Harmony
{
	[HarmonyPatch(typeof(CarScreenLogic), "Start")]
	internal static class CarScreenLogic__Start
	{
		[HarmonyPostfix]
		internal static void Postfix(CarScreenLogic __instance)
		{
			__instance.StartCoroutine(LoadScreenTextures(__instance));
		}

		internal static IEnumerator LoadScreenTextures(CarScreenLogic __instance)
		{
			const int TEX_SIZE = 1024;

			GameObject container = __instance.gameObject.transform.Find("CircleParent/CarVoiceObjects").gameObject;
			GameObject prefab = container.GetChild(6);

			Settings cfg = new Settings("ScreenTextures");

			if (cfg.ContainsKey("url"))
			{
				foreach (string url in cfg.GetItem<List<string>>("items"))
				{
					Mod.Instance.Logger.Info($"Loading \"{url}\"");

					WWW web = new WWW(url);

					yield return web;

					CreateTextureObject(prefab, container, web.texture);
				}
			}

			if (cfg.ContainsKey("files"))
			{
				FileSystem fs = new FileSystem();

				foreach (string file in cfg.GetItem<List<string>>("files"))
				{
					string path = Path.Combine(fs.VirtualFileSystemRoot, file);

					if (!FileEx.Exists(path))
					{
						continue;
					}

					Mod.Instance.Logger.Info($"Loading \"{path}\"");

					GameObject newScreenObj = CreateTextureObject(prefab, container, Resource.LoadTextureFromFile(path, TEX_SIZE, TEX_SIZE));
					newScreenObj.name += $" [{file}]";
				}
			}

			__instance.specialCarVoiceObjects_ = container.GetChildren();

			foreach (GameObject obj in __instance.specialCarVoiceObjects_)
			{
				if (obj)
				{
					Mod.Instance.Logger.Warning(obj.name);
				}
			}
		}

		internal static GameObject CreateTextureObject(GameObject prefab, GameObject parent, Texture image)
		{
			GameObject newObj = GameObject.Instantiate(prefab, parent.transform);
			newObj.name = "CustomTextureObject (Modded)";

			newObj.SetLayer(parent.GetLayer());

			newObj.GetComponent<MeshRenderer>().material.mainTexture = image;

			return newObj;
		}
	}
}
