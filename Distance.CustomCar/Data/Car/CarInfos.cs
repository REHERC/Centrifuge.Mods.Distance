using Distance.CustomCar.Data.Materials;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public class CarInfos
	{
		public Dictionary<string, MaterialInfos> materials = new Dictionary<string, MaterialInfos>();
		public GameObject boostJet = null;
		public GameObject wingJet = null;
		public GameObject rotationJet = null;
		public GameObject wingTrail = null;

		public GameObject baseCar = null;
		public CarColors defaultColors;

		public void CollectInfos()
		{
			GetBaseCar();
			GetJetsAndTrail();
			GetMaterials();
		}

		private void GetBaseCar()
		{
			var prefab = G.Sys.ProfileManager_.carInfos_[0].prefabs_.carPrefab_;
			if (prefab == null)
			{
				Mod.Instance.Errors.Add("Can't find the refractor base car prefab");
				return;
			}
			baseCar = prefab;
			defaultColors = G.Sys.ProfileManager_.carInfos_[0].colors_;
		}

		private void GetJetsAndTrail()
		{
			if (baseCar == null)
			{
				return;
			}

			foreach (var jet in baseCar.GetComponentsInChildren<JetFlame>())
			{
				var name = jet.gameObject.name;
				switch (name)
				{
					case "BoostJetFlameCenter":
						boostJet = jet.gameObject;
						break;
					case "JetFlameBackLeft":
						rotationJet = jet.gameObject;
						break;

					case "WingJetFlameLeft1":
						wingJet = jet.gameObject;
						break;
				}
			}

			wingTrail = baseCar.GetComponentInChildren<WingTrail>().gameObject;

			if (boostJet == null)
			{
				Mod.Instance.Errors.Add("No valid BoostJet found on Refractor");
			}

			if (rotationJet == null)
			{
				Mod.Instance.Errors.Add("No valid RotationJet found on Refractor");
			}

			if (wingJet == null)
			{
				Mod.Instance.Errors.Add("No valid WingJet found on Refractor");
			}

			if (wingTrail == null)
			{
				Mod.Instance.Errors.Add("No valid WingTrail found on Refractor");
			}
		}

		private void GetMaterials()
		{
			List<MaterialPropertyInfo> materialsNames = new List<MaterialPropertyInfo>
			{
				new MaterialPropertyInfo("Custom/LaserCut/CarPaint", "carpaint", 5, -1, -1),
				new MaterialPropertyInfo("Custom/LaserCut/CarWindow", "carwindow", -1, 218, 219),
				new MaterialPropertyInfo("Custom/Reflective/Bump Glow LaserCut", "wheel", 5, 218, 255),
				new MaterialPropertyInfo("Custom/LaserCut/CarPaintBump", "carpaintbump", 5, 218, -1),
				new MaterialPropertyInfo("Custom/Reflective/Bump Glow Interceptor Special", "interceptor", 5, 218, 255),
				new MaterialPropertyInfo("Custom/LaserCut/CarWindowTrans2Sided", "transparentglow", -1, 218, 219)
			};

			foreach (var c in G.Sys.ProfileManager_.carInfos_)
			{
				var prefab = c.prefabs_.carPrefab_;
				foreach (var renderer in prefab.GetComponentsInChildren<Renderer>())
				{
					foreach (var mat in renderer.materials)
					{
						foreach (var key in materialsNames)
						{
							if (materials.ContainsKey(key.name))
							{
								continue;
							}

							if (mat.shader.name == key.shaderName)
							{
								var materialInfos = new MaterialInfos
								{
									material = mat,
									diffuseIndex = key.diffuseIndex,
									normalIndex = key.normalIndex,
									emitIndex = key.emitIndex
								};
								materials.Add(key.name, materialInfos);
							}
						}
					}
				}
			}

			foreach (var mat in materialsNames)
			{
				if (!materials.ContainsKey(mat.name))
				{
					Mod.Instance.Errors.Add("Can't find the material: " + mat.name + " - shader: " + mat.shaderName);
				}
			}

			materials.Add("donotreplace", new MaterialInfos());
		}
	}
}
