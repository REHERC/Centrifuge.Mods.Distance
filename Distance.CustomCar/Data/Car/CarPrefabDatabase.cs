using Reactor.API.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public class CarPrefabDatabase : List<GameObject>
	{
		private readonly CarFactory factory_;

		public CarPrefabDatabase(CarFactory factory)
		{
			factory_ = factory;
		}

		public void LoadAll()
		{
			foreach (KeyValuePair<FileInfo, Assets> item in factory_.Assets)
			{
				Assets assets = item.Value;
				AssetBundle bundle = assets.Bundle as AssetBundle;

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
					factory_.Errors.Add($"Can't find a prefab in the asset bundle", "Custom assets", item.Key.FullName);
				}
				else
				{
					Add(carPrefab);
				}
			}
		}
	}
}
