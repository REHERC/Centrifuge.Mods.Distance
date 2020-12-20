using Reactor.API.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public class CarAssetBundles : Dictionary<string, AssetBundle>
	{
		private readonly DirectoryInfo assetsDirectory_;
		private readonly CarFactory factory_;

		public CarAssetBundles(DirectoryInfo assetsDirectory, CarFactory factory)
		{
			assetsDirectory_ = assetsDirectory;
			factory_ = factory;
		}

		public void LoadAll()
		{
			foreach (FileInfo file in assetsDirectory_.GetFiles("*", SearchOption.AllDirectories).OrderBy(x => x.Name))
			{
				LoadAssetsFile(file);
			}
		}

		public void LoadAssetsFile(FileInfo file)
		{
			string fileName = file.FullName.Normalize().ToLower().Replace(Path.DirectorySeparatorChar, '/');
			
			if (!ContainsKey(fileName))
			{
				try
				{
					Assets assets = Assets.FromUnsafePath(file.FullName);
					Add(fileName, assets.Bundle as AssetBundle);
				}
				catch (Exception ex)
				{
					factory_.Errors.Add(ex.Message, "Custom assets loader", file.FullName);
				}
			}
		}
	}
}