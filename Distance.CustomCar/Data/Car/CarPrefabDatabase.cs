using Distance.CustomCar.Data.Error;
using Reactor.API.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public class CarPrefabDatabase : List<GameObject>
	{
		private readonly DirectoryInfo assetsDirectory_;
		private readonly ErrorList errors_;

		public CarPrefabDatabase(DirectoryInfo assetsDirectory, ErrorList errors)
		{
			assetsDirectory_ = assetsDirectory;
			errors_ = errors;
		}

		public void LoadAll()
		{
			Mod.Instance.Logger.Warning($"Scanning {assetsDirectory_.FullName}...");
			foreach (FileInfo file in assetsDirectory_.GetFiles())
			{
				Mod.Instance.Logger.Warning($"Found {file.FullName}...");

				Assets assetsFile = Assets.FromUnsafePath(file.FullName);
				AssetBundle bundle = assetsFile.Bundle as AssetBundle;

				GameObject carPrefab = null;

				foreach (string assetName in bundle.GetAllAssetNames())
				{
					if (assetName.EndsWith(".prefab", StringComparison.InvariantCultureIgnoreCase))
					{
						carPrefab = bundle.LoadAsset<GameObject>(assetName);
						break;
					}
				}

				if (!carPrefab)
				{
					errors_.Add($"Can't find a prefab in the {file.Name} asset bundle", "Custom assets");
				}
				else
				{
					Add(carPrefab);
				}
			}
		}
	}
}
