using Distance.CustomCar.Data.Materials;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public class CarInfos
	{
		public const string BoostJetObjectName = "BoostJetFlameCenter";
		public const string RotationJetObjectName = "JetFlameBackLeft";
		public const string WingJetObjectName = "WingJetFlameLeft1";

		public static ProfileManager ProfileManager => G.Sys.ProfileManager_;

		public static CarInfo DefaultCar => ProfileManager.CarInfos_?[0];

		public Dictionary<string, MaterialInfos> materials;
		public GameObject baseCar;
		public GameObject boostJet;
		public GameObject rotationJet;
		public GameObject wingJet;
		public WingTrail wingTrail;
		public CarColors defaultColors;

		public bool CollectInformations()
		{
			return GetBaseCar() 
				&& GetJetsAndTrails()
				&& GetMaterials();
		}

		private bool GetBaseCar()
		{
			GameObject prefab = DefaultCar.prefabs_.carPrefab_;

			if (!prefab)
			{
				Mod.Instance.Errors.Add("Can't find the Spectrum base car prefab", "Game assets");
				return false;
			}

			baseCar = prefab;
			defaultColors = DefaultCar.colors_;

			return true;
		}

		private bool GetJetsAndTrails()
		{
			foreach (JetFlame flame in baseCar.GetComponentsInChildren<JetFlame>())
			{
				switch (flame.name)
				{
					case BoostJetObjectName:
						boostJet = flame.gameObject;
						break;
					case RotationJetObjectName:
						rotationJet = flame.gameObject;
						break;
					case WingJetObjectName:
						wingJet = flame.gameObject;
						break;
				}
			}

			bool allPrefabsValid = true;

			if (!boostJet)
			{
				Mod.Instance.Errors.Add("No valid BoostJet found on Spectrum", "Game assets");
				allPrefabsValid = false;
			}

			if (!rotationJet)
			{
				Mod.Instance.Errors.Add("No valid RotationJet found on Spectrum", "Game assets");
				allPrefabsValid = false;
			}

			if (!wingJet)
			{
				Mod.Instance.Errors.Add("No valid WingJet found on Spectrum", "Game assets");
				allPrefabsValid = false;
			}

			if (!wingTrail)
			{
				Mod.Instance.Errors.Add("No valid WingTrail found on Spectrum", "Game assets");
				allPrefabsValid = false;
			}


			return allPrefabsValid;
		}
	
		private bool GetMaterials()
		{
			materials = new Dictionary<string, MaterialInfos>();

			foreach (CarInfo car in ProfileManager.CarInfos_)
			{
				GameObject prefab = car.prefabs_.carPrefab_;

				foreach (Renderer renderer in prefab.GetComponentsInChildren<Renderer>())
				{
					foreach (Material material in renderer.materials)
					{
						foreach (MaterialPropertyInfo properties in MaterialPropertyInfo.CommonCarMaterials)
						{
							if (!materials.ContainsKey(properties.materialName) && string.Equals(material.shader.name, properties.shaderName, StringComparison.InvariantCultureIgnoreCase))
							{
								MaterialInfos materialInfos = new MaterialInfos(material, properties.diffuseID, properties.emitID, properties.normalID);
								materials.Add(properties.materialName, materialInfos);
							}
						}
					}
				}
			}

			return true;
		}
	}
}
