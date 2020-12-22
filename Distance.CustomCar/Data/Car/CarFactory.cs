using Distance.CustomCar.Data.Error;
using Reactor.API;
using System.Collections.Generic;

namespace Distance.CustomCar.Data.Car
{
	public class CarFactory
	{
		public ErrorList Errors { get; internal set; }

		public CarAssetBundles Assets { get; internal set; }

		public AssetBundleDatabase Bundles { get; internal set; }

		public CarInfos Infos { get; internal set; }

		public CarBuilder Builder { get; internal set; }

		public CarFactory(ErrorList errors)
		{
			Errors = errors;

			Assets = new CarAssetBundles(Mod.Instance.GetLocalFolder(Defaults.PrivateAssetsDirectory), this);
			Bundles = new AssetBundleDatabase(this);
			Infos = new CarInfos();
			Builder = new CarBuilder(this);
		}

		public void LoadAssets()
		{
			Assets.LoadAll();
		}

		public void MakeCars()
		{
			if (Infos.CollectInformations())
			{
				Bundles.Clear();
				Bundles.LoadAll();

				Builder.Clear();

				#if USE_INTERNAL_ASSETS
				Builder.CreateInternalCars();
				#endif

				Builder.CreateCars();
			}
		}

		public void RegisterAll()
		{
			if (Builder.Count > 0)
			{
				AddCarsToGame();
			}

			var infos = G.Sys.ProfileManager_.CarInfos_;
			Mod.Instance.Logger.Warning($"{infos.Length} cars in total");

			foreach (var car in G.Sys.ProfileManager_.knownCars_)
			{
				Mod.Instance.Logger.Info($"{car.Value}\t{car.Key}");
			}
		}

		private void AddCarsToGame()
		{
			ProfileManager profileManager = G.Sys.ProfileManager_;
			CarInfo[] oldCars = profileManager.carInfos_.ToArray();

			profileManager.carInfos_ = new CarInfo[Mod.DefaultCarCount + Builder.Count];

			Dictionary<string, int> unlocked = profileManager.unlockedCars_;
			Dictionary<string, int> knowCars = profileManager.knownCars_;

			for (int carIndex = 0; carIndex < profileManager.carInfos_.Length; carIndex++)
			{
				if (carIndex < Mod.DefaultCarCount)
				{
					profileManager.carInfos_[carIndex] = oldCars[carIndex];
					continue;
				}

				int index = carIndex - Mod.DefaultCarCount;

				CarInfo car = new CarInfo
				{
					name_ = Builder[index].carPrefab.name,
					prefabs_ = new CarPrefabs
					{
						carPrefab_ = Builder[index].carPrefab
					},
					colors_ = Builder[index].carColors
				};

				profileManager.carInfos_[carIndex] = car;
				unlocked[car.name_] = carIndex;
				knowCars[car.name_] = carIndex;
			}

			CarColors[] carColors = new CarColors[oldCars.Length + Builder.Count];
			for (int colorIndex = 0; colorIndex < carColors.Length; colorIndex++)
			{
				carColors[colorIndex] = G.Sys.ProfileManager_.carInfos_[colorIndex].colors_;
			}

			for (int profileIndex = 0; profileIndex < profileManager.ProfileCount_; profileIndex++)
			{
				Profile profile = profileManager.GetProfile(profileIndex);

				CarColors[] oldColorList = profile.carColorsList_;
				for (int j = 0; j < oldColorList.Length && j < carColors.Length; j++)
				{
					carColors[j] = oldColorList[j];
				}

				profile.carColorsList_ = carColors;
			}
		}
	}
}