using Reactor.API.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public class CarPrefabDatabase : Dictionary<string, GameObject>
	{
		private readonly CarFactory factory_;

		public CarPrefabDatabase(CarFactory factory)
		{
			factory_ = factory;
		}

		public void LoadAll()
		{
			foreach (KeyValuePair<string, AssetBundle> item in factory_.Assets)
			{
				try
				{
					AssetBundle bundle = item.Value;

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
						factory_.Errors.Add($"Can't find a prefab in the asset bundle", "Custom assets", item.Key);
					}
					else
					{
						Add(item.Key, carPrefab);
					}
				}
				catch (Exception ex)
				{
					factory_.Errors.Add($"Something went wrong when loading an assets file\n{ex.Message}", "Custom assets", item.Key);
				}
			}
		}
	}
}
