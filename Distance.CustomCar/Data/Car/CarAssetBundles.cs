using Reactor.API.Storage;
using System.Collections.Generic;
using System.IO;

namespace Distance.CustomCar.Data.Car
{
	public class CarAssetBundles : Dictionary<FileInfo, Assets>
	{
		private readonly DirectoryInfo assetsDirectory_;

		public CarAssetBundles(DirectoryInfo assetsDirectory)
		{
			assetsDirectory_ = assetsDirectory;
		}

		public void LoadAll()
		{
			foreach (FileInfo file in assetsDirectory_.GetFiles())
			{
				LoadAssetsFile(file);
			}
		}

		public void LoadAssetsFile(FileInfo file)
		{
			if (!ContainsKey(file))
			{
				Add(file, Assets.FromUnsafePath(file.FullName));
			}
		}
	}
}
