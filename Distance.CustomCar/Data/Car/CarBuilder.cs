using Distance.CustomCar.Data.Materials;
using Reactor.API;
using Reactor.API.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public class CarBuilder
	{
		private CarInfos infos_;

		public void CreateCars(CarInfos infos)
		{
			infos_ = infos;
			Dictionary<string, GameObject> cars = LoadAssetsBundles();

			List<CreateCarReturnInfos> carsInfos = new List<CreateCarReturnInfos>();

			foreach (KeyValuePair<string, GameObject> car in cars)
			{
				try
				{
					Mod.Instance.Logger.Info($"Creating car prefab for {car.Key} ...");
					CreateCarReturnInfos data = CreateCar(car.Value);

					string fileName = Path.GetFileNameWithoutExtension(car.Key.Substring(0, car.Key.LastIndexOf('(') - 1));
					data.car.name = fileName;

					carsInfos.Add(data);
				}
				catch (Exception ex)
				{
					Mod.Instance.Errors.Add($"Could not load car prefab: {car.Key}");
					Mod.Instance.Errors.Add(ex);
				}
			}

			RegisterCars(carsInfos);
		}

		private void RegisterCars(List<CreateCarReturnInfos> carsInfos)
		{
			Mod.Instance.Logger.Info($"Registering {carsInfos.Count} car(s)...");

			ProfileManager profileManager = G.Sys.ProfileManager_;
			CarInfo[] oldCars = profileManager.carInfos_.ToArray();
			profileManager.carInfos_ = new CarInfo[oldCars.Length + carsInfos.Count];

			ref Dictionary<string, int> unlocked = ref profileManager.unlockedCars_;
			ref Dictionary<string, int> knowCars = ref profileManager.knownCars_;

			for (int carIndex = 0; carIndex < profileManager.carInfos_.Length; carIndex++)
			{
				if (carIndex < oldCars.Length)
				{
					profileManager.carInfos_[carIndex] = oldCars[carIndex];
					continue;
				}

				int infoIndex = carIndex - oldCars.Length;

				CarInfo car = new CarInfo
				{
					name_ = carsInfos[infoIndex].car.name,
					prefabs_ = new CarPrefabs
					{
						carPrefab_ = carsInfos[infoIndex].car
					},
					colors_ = carsInfos[infoIndex].colors
				};

				if (!knowCars.ContainsKey(car.name_) && !unlocked.ContainsKey(car.name_))
				{
					unlocked.Add(car.name_, carIndex);
					knowCars.Add(car.name_, carIndex);
				}
				else
				{
					Mod.Instance.Errors.Add($"A car with the name {car.name_} is already registered, rename the car file if they're the same.");
					Mod.Instance.Logger.Warning($"Generating unique name for car {car.name_}");

					string uniqueID = $"#{Guid.NewGuid():B}";
					Mod.Instance.Logger.Info($"Using GUID: {uniqueID}");

					car.name_ = $"[FFFF00]![-] {car.name_} {uniqueID}";

					unlocked.Add(car.name_, carIndex);
					knowCars.Add(car.name_, carIndex);
				}

				profileManager.carInfos_[carIndex] = car;
			}

			CarColors[] carColors = new CarColors[oldCars.Length + carsInfos.Count];
			for (int colorIndex = 0; colorIndex < carColors.Length; colorIndex++)
			{
				carColors[colorIndex] = G.Sys.ProfileManager_.carInfos_[colorIndex].colors_;
			}
			for (int profileIndex = 0; profileIndex < profileManager.ProfileCount_; profileIndex++)
			{
				Profile profile = profileManager.GetProfile(profileIndex);

				CarColors[] oldColorList = profile.carColorsList_;

				for (int oldColorIndex = 0; oldColorIndex < oldColorList.Length && oldColorIndex < carColors.Length; oldColorIndex++)
				{
					carColors[oldColorIndex] = oldColorList[oldColorIndex];
				}

				profile.carColorsList_ = carColors;
			}
		}

		private Dictionary<string, GameObject> LoadAssetsBundles()
		{
			Dictionary<string, GameObject> assetsList = new Dictionary<string, GameObject>();
			DirectoryInfo assetsDirectory = GetLocalFolder(Defaults.PrivateAssetsDirectory);
			DirectoryInfo globalCarsDirectory = new DirectoryInfo(Path.Combine(Resource.personalDistanceDirPath_, "CustomCars"));

			if (!globalCarsDirectory.Exists)
			{
				try
				{
					globalCarsDirectory.Create();
				}
				catch (Exception ex)
				{
					Mod.Instance.Errors.Add($"Could not create the following folder: {globalCarsDirectory.FullName}");
					Mod.Instance.Errors.Add(ex);
				}
			}

			foreach (FileInfo assetsFile in assetsDirectory.GetFiles("*", SearchOption.AllDirectories).Concat(globalCarsDirectory.GetFiles("*", SearchOption.AllDirectories)).OrderBy(x => x.Name))
			{
				try
				{
					Assets assets = Assets.FromUnsafePath(assetsFile.FullName);
					AssetBundle bundle = assets.Bundle as AssetBundle;

					int foundPrefabCount = 0;

					foreach (string assetName in from name in bundle.GetAllAssetNames() where name.EndsWith(".prefab", StringComparison.InvariantCultureIgnoreCase) select name)
					{
						GameObject carPrefab = bundle.LoadAsset<GameObject>(assetName);

						string assetKey = $"{assetsFile.FullName} ({assetName})";

						if (!assetsList.ContainsKey(assetKey))
						{
							assetsList.Add(assetKey, carPrefab);
							foundPrefabCount++;
						}
					}

					if (foundPrefabCount == 0)
					{
						Mod.Instance.Errors.Add($"Can't find a prefab in the asset bundle: {assetsFile.FullName}");
					}
				}
				catch (Exception ex)
				{
					Mod.Instance.Errors.Add($"Could not load assets file: {assetsFile.FullName}");
					Mod.Instance.Errors.Add(ex);
				}
			}

			return assetsList;
		}

		public DirectoryInfo GetLocalFolder(string dir)
		{
			FileSystem files = new FileSystem();
			return new DirectoryInfo(Path.GetDirectoryName(Path.Combine(files.RootDirectory, dir + (dir.EndsWith($"{Path.DirectorySeparatorChar}") ? string.Empty : $"{Path.DirectorySeparatorChar}"))));
		}

		private CreateCarReturnInfos CreateCar(GameObject car)
		{
			CreateCarReturnInfos infos = new CreateCarReturnInfos();

			GameObject obj = GameObject.Instantiate(infos_.baseCar);
			obj.name = car.name;
			GameObject.DontDestroyOnLoad(obj);
			obj.SetActive(false);

			RemoveOldCar(obj);
			GameObject newCar = AddNewCarOnPrefab(obj, car);
			SetCarDatas(obj, newCar);
			infos.car = obj;

			infos.colors = LoadDefaultColors(newCar);

			return infos;
		}

		private void RemoveOldCar(GameObject obj)
		{
			List<GameObject> wheelsToRemove = new List<GameObject>();
			for (int childIndex = 0; childIndex < obj.transform.childCount; childIndex++)
			{
				GameObject childObject = obj.transform.GetChild(childIndex).gameObject;
				if (childObject.name.IndexOf("wheel", StringComparison.InvariantCultureIgnoreCase) >= 0)
				{
					wheelsToRemove.Add(childObject);
				}
			}

			if (wheelsToRemove.Count != 4)
			{
				Mod.Instance.Errors.Add($"Found {wheelsToRemove.Count} wheels on base prefabs, expected 4");
			}

			Transform refractor = obj.transform.Find("Refractor");
			if (refractor == null)
			{
				Mod.Instance.Errors.Add("Can't find the Refractor object on the base car prefab");
				return;
			}

			GameObject.Destroy(refractor.gameObject);
			foreach (GameObject wheel in wheelsToRemove)
			{
				UnityEngine.Object.Destroy(wheel);
			}
		}

		private GameObject AddNewCarOnPrefab(GameObject obj, GameObject car)
		{
			return UnityEngine.Object.Instantiate(car, obj.transform);
		}

		private void SetCarDatas(GameObject obj, GameObject car)
		{
			SetColorChanger(obj.GetComponent<ColorChanger>(), car);
			SetCarVisuals(obj.GetComponent<CarVisuals>(), car);
		}

		private void SetColorChanger(ColorChanger colorChanger, GameObject car)
		{
			if (colorChanger == null)
			{
				Mod.Instance.Errors.Add("Can't find the ColorChanger component on the base car");
				return;
			}

			colorChanger.rendererChangers_ = new ColorChanger.RendererChanger[0];
			foreach (Renderer renderer in car.GetComponentsInChildren<Renderer>())
			{
				ReplaceMaterials(renderer);
				if (colorChanger != null)
				{
					AddMaterialColorChanger(colorChanger, renderer.transform);
				}
			}
		}

		private void ReplaceMaterials(Renderer renderer)
		{
			string[] materialNames = new string[renderer.materials.Length];

			for (int materialNameIndex = 0; materialNameIndex < materialNames.Length; materialNameIndex++)
			{
				materialNames[materialNameIndex] = "wheel";
			}

			List<MaterialPropertyExport>[] materialProperties = new List<MaterialPropertyExport>[renderer.materials.Length];
			for (int propertyIndex = 0; propertyIndex < materialProperties.Length; propertyIndex++)
			{
				materialProperties[propertyIndex] = new List<MaterialPropertyExport>();
			}

			FillMaterialInfos(renderer, materialNames, materialProperties);

			Material[] materials = renderer.materials.ToArray();
			for (int materialIndex = 0; materialIndex < renderer.materials.Length; materialIndex++)
			{
				if (!infos_.materials.TryGetValue(materialNames[materialIndex], out MaterialInfos materialInfo))
				{
					Mod.Instance.Errors.Add($"Can't find the material {materialNames[materialIndex]} on {renderer.gameObject.FullName()}");
					continue;
				}

				if (materialInfo == null || materialInfo.material == null)
				{
					continue;
				}

				Material material = UnityEngine.Object.Instantiate(materialInfo.material);
				if (materialInfo.diffuseIndex >= 0)
				{
					material.SetTexture(materialInfo.diffuseIndex, renderer.materials[materialIndex].GetTexture("_MainTex"));
				}

				if (materialInfo.normalIndex >= 0)
				{
					material.SetTexture(materialInfo.normalIndex, renderer.materials[materialIndex].GetTexture("_BumpMap"));
				}

				if (materialInfo.emitIndex >= 0)
				{
					material.SetTexture(materialInfo.emitIndex, renderer.materials[materialIndex].GetTexture("_EmissionMap"));
				}

				foreach (MaterialPropertyExport property in materialProperties[materialIndex])
				{
					CopyMaterialProperty(renderer.materials[materialIndex], material, property);
				}

				materials[materialIndex] = material;
			}

			renderer.materials = materials;
		}

		private void FillMaterialInfos(Renderer renderer, string[] matNames, List<MaterialPropertyExport>[] materialProperties)
		{
			int childCount = renderer.transform.childCount;

			for (int childIndex = 0; childIndex < childCount; childIndex++)
			{
				string name = renderer.transform.GetChild(childIndex).name.ToLower();
				if (!name.StartsWith("#"))
					{
					continue;
				}
				name = name.Remove(0, 1);

				string[] arguments = name.Split(';');
				if (arguments.Length == 0)
					{
					continue;
				}

				if (arguments[0].Contains("mat"))
				{
					if (arguments.Length != 3)
					{
						Mod.Instance.Errors.Add($"{arguments[0]} property on {renderer.gameObject.FullName()} must have 2 arguments");
						continue;
					}

					if (!int.TryParse(arguments[1], out int index))
					{
						Mod.Instance.Errors.Add($"First argument of {arguments[0]} on {renderer.gameObject.FullName()} property must be a number");
						continue;
					}

					if (index < matNames.Length)
					{
						matNames[index] = arguments[2];
					}
				}
				else if (arguments[0].Contains("export"))
				{
					if (arguments.Length != 5)
					{
						Mod.Instance.Errors.Add($"{arguments[0]} property on {renderer.gameObject.FullName()} must have 4 arguments");
						continue;
					}

					if (!int.TryParse(arguments[1], out int index))
					{
						Mod.Instance.Errors.Add($"First argument of {arguments[0]} on {renderer.gameObject.FullName()} property must be a number");
						continue;
					}

					if (index >= matNames.Length)
						{
						continue;
					}

					MaterialPropertyExport property = new MaterialPropertyExport();
					bool found = false;

					foreach (PropertyType propertyType in Enum.GetValues(typeof(PropertyType)))
					{
						if (arguments[2] == propertyType.ToString().ToLower())
						{
							found = true;
							property.type = propertyType;
							break;
						}
					}

					if (!found)
					{
						Mod.Instance.Errors.Add($"The property {arguments[2]} on {renderer.gameObject.FullName()} is not valid");
						continue;
					}

					if (!int.TryParse(arguments[3], out property.fromID))
					{
						property.fromName = arguments[3];
					}

					if (!int.TryParse(arguments[4], out property.toID))
					{
						property.toName = arguments[4];
					}

					materialProperties[index].Add(property);
				}
			}
		}

		private void CopyMaterialProperty(Material from, Material to, MaterialPropertyExport property)
		{
			int fromID = property.fromID;
			if (fromID == -1)
			{
				fromID = Shader.PropertyToID(property.fromName);
			}

			int toID = property.toID;
			if (toID == -1)
			{
				toID = Shader.PropertyToID(property.toName);
			}

			switch (property.type)
			{
				case PropertyType.Color:
					to.SetColor(toID, from.GetColor(fromID));
					break;
				case PropertyType.ColorArray:
					to.SetColorArray(toID, from.GetColorArray(fromID));
					break;
				case PropertyType.Float:
					to.SetFloat(toID, from.GetFloat(fromID));
					break;
				case PropertyType.FloatArray:
					to.SetFloatArray(toID, from.GetFloatArray(fromID));
					break;
				case PropertyType.Int:
					to.SetInt(toID, from.GetInt(fromID));
					break;
				case PropertyType.Matrix:
					to.SetMatrix(toID, from.GetMatrix(fromID));
					break;
				case PropertyType.MatrixArray:
					to.SetMatrixArray(toID, from.GetMatrixArray(fromID));
					break;
				case PropertyType.Texture:
					to.SetTexture(toID, from.GetTexture(fromID));
					break;
				case PropertyType.Vector:
					to.SetVector(toID, from.GetVector(fromID));
					break;
				case PropertyType.VectorArray:
					to.SetVectorArray(toID, from.GetVectorArray(fromID));
					break;
			}
		}

		private void AddMaterialColorChanger(ColorChanger colorChanger, Transform transform)
		{
			Renderer renderer = transform.GetComponent<Renderer>();
			if (renderer == null)
			{
				return;
			}

			List<ColorChanger.UniformChanger> uniformChangers = new List<ColorChanger.UniformChanger>();

			for (int i = 0; i < transform.childCount; i++)
			{
				GameObject o = transform.GetChild(i).gameObject;
				string name = o.name.ToLower();
				if (!name.StartsWith("#"))
					{
					continue;
				}

				name = name.Remove(0, 1); //remove #

				string[] arguments = name.Split(';');
				if (arguments.Length == 0)
					{
					continue;
				}

				if (!arguments[0].Contains("color"))
				{
					continue;
				}

				if (arguments.Length != 6)
				{
					Mod.Instance.Errors.Add($"{arguments[0]} property on {transform.gameObject.FullName()} must have 5 arguments");
					continue;
				}

				ColorChanger.UniformChanger uniformChanger = new ColorChanger.UniformChanger();
				int.TryParse(arguments[1], out int materialIndex);
				uniformChanger.materialIndex_ = materialIndex;
				uniformChanger.colorType_ = ColorType(arguments[2]);
				uniformChanger.name_ = UniformName(arguments[3]);

				float.TryParse(arguments[4], out float multiplier);
				uniformChanger.mul_ = multiplier;
				uniformChanger.alpha_ = string.Equals(arguments[5], "true", StringComparison.InvariantCultureIgnoreCase);

				uniformChangers.Add(uniformChanger);
			}

			if (uniformChangers.Count == 0)
			{
				return;
			}

			ColorChanger.RendererChanger renderChanger = new ColorChanger.RendererChanger
			{
				renderer_ = renderer,
				uniformChangers_ = uniformChangers.ToArray()
			};

			List<ColorChanger.RendererChanger> renders = colorChanger.rendererChangers_.ToList();
			renders.Add(renderChanger);
			colorChanger.rendererChangers_ = renders.ToArray();
		}

		private ColorChanger.ColorType ColorType(string name)
		{
			name = name.ToLower();
			switch (name)
			{
				case "primary":
					return ColorChanger.ColorType.Primary;
				case "secondary":
					return ColorChanger.ColorType.Secondary;
				case "glow":
					return ColorChanger.ColorType.Glow;
				case "sparkle":
					return ColorChanger.ColorType.Sparkle;
			}

			return ColorChanger.ColorType.Primary;
		}

		private MaterialEx.SupportedUniform UniformName(string name)
		{
			name = name.ToLower();
			switch (name)
			{
				case "color":
					return MaterialEx.SupportedUniform._Color;
				case "color2":
					return MaterialEx.SupportedUniform._Color2;
				case "emitcolor":
					return MaterialEx.SupportedUniform._EmitColor;
				case "reflectcolor":
					return MaterialEx.SupportedUniform._ReflectColor;
				case "speccolor":
					return MaterialEx.SupportedUniform._SpecColor;
			}

			return MaterialEx.SupportedUniform._Color;
		}

		private void SetCarVisuals(CarVisuals visuals, GameObject car)
		{
			if (visuals == null)
			{
				Mod.Instance.Errors.Add("Can't find the CarVisuals component on the base car");
				return;
			}

			SkinnedMeshRenderer skinned = car.GetComponentInChildren<SkinnedMeshRenderer>();
			MakeMeshSkinned(skinned);
			visuals.carBodyRenderer_ = skinned;

			List<JetFlame> boostJets = new List<JetFlame>();
			List<JetFlame> wingJets = new List<JetFlame>();
			List<JetFlame> rotationJets = new List<JetFlame>();

			PlaceJets(car, boostJets, wingJets, rotationJets);
			visuals.boostJetFlames_ = boostJets.ToArray();
			visuals.wingJetFlames_ = wingJets.ToArray();
			visuals.rotationJetFlames_ = rotationJets.ToArray();
			visuals.driverPosition_ = FindCarDriver(car.transform);

			PlaceCarWheelsVisuals(visuals, car);
		}

		private void MakeMeshSkinned(SkinnedMeshRenderer renderer)
		{
			Mesh mesh = renderer.sharedMesh;
			if (mesh == null)
			{
				Mod.Instance.Errors.Add($"The mesh on {renderer.gameObject.FullName()} is null");
				return;
			}

			if (!mesh.isReadable)
			{
				Mod.Instance.Errors.Add($"Can't read the car mesh {mesh.name} on {renderer.gameObject.FullName()}You must allow reading on it's unity inspector !");
				return;
			}

			if (mesh.vertices.Length == mesh.boneWeights.Length)
			{
				return;
			}

			BoneWeight[] bones = new BoneWeight[mesh.vertices.Length];
			for (int i = 0; i < bones.Length; i++)
			{
				bones[i].weight0 = 1;
			}

			mesh.boneWeights = bones;
			Transform transform = renderer.transform;
			mesh.bindposes = (new Matrix4x4[1]
			{
				transform.worldToLocalMatrix * renderer.transform.localToWorldMatrix
			});
			renderer.bones = new Transform[1]
			{
				transform
			};
		}

		private void PlaceJets(GameObject obj, List<JetFlame> boostJets, List<JetFlame> wingJets, List<JetFlame> rotationJets)
		{
			int childCount = obj.transform.childCount;
			for (int childIndex = 0; childIndex < childCount; childIndex++)
			{
				GameObject child = obj.GetChild(childIndex).gameObject;
				string name = child.name.ToLower();
				if (infos_.boostJet != null && name.Contains("boostjet"))
				{
					GameObject jet = GameObject.Instantiate(infos_.boostJet, child.transform);
					jet.transform.localPosition = Vector3.zero;
					jet.transform.localRotation = Quaternion.identity;
					boostJets.Add(jet.GetComponentInChildren<JetFlame>());
				}
				else if (infos_.wingJet != null && name.Contains("wingjet"))
				{
					GameObject jet = GameObject.Instantiate(infos_.wingJet, child.transform);
					jet.transform.localPosition = Vector3.zero;
					jet.transform.localRotation = Quaternion.identity;
					wingJets.Add(jet.GetComponentInChildren<JetFlame>());
					wingJets.Last().rotationAxis_ = JetDirection(child.transform);
				}
				else if (infos_.rotationJet != null && name.Contains("rotationjet"))
				{
					GameObject jet = GameObject.Instantiate(infos_.rotationJet, child.transform);
					jet.transform.localPosition = Vector3.zero;
					jet.transform.localRotation = Quaternion.identity;
					rotationJets.Add(jet.GetComponentInChildren<JetFlame>());
					rotationJets.Last().rotationAxis_ = JetDirection(child.transform);
				}
				else if (infos_.wingTrail != null && name.Contains("wingtrail"))
				{
					GameObject trail = GameObject.Instantiate(infos_.wingTrail, child.transform);
					trail.transform.localPosition = Vector3.zero;
					trail.transform.localRotation = Quaternion.identity;
				}
				else
				{
					PlaceJets(child, boostJets, wingJets, rotationJets);
				}
			}
		}

		private Vector3 JetDirection(Transform transform)
		{
			int childCount = transform.childCount;
			for (int childIndex = 0; childIndex < childCount; childIndex++)
			{
				string name = transform.GetChild(childIndex).gameObject.name.ToLower();
				if (!name.StartsWith("#"))
				{
					continue;
				}
				name = name.Remove(0, 1);

				string[] arguments = name.Split(';');
				if (arguments.Length == 0)
				{
					continue;
				}

				if (!arguments[0].Contains("dir"))
					{
					continue;
				}

				if (arguments.Length < 2)
					{
					continue;
				}

				if (arguments[1] == "front")
				{
					return new Vector3(-1, 0, 0);
				}

				if (arguments[1] == "back")
				{
					return new Vector3(1, 0, 0);
				}

				if (arguments[1] == "left")
				{
					return new Vector3(0, 1, -1);
				}

				if (arguments[1] == "right")
				{
					return new Vector3(0, -1, 1);
				}

				if (arguments.Length != 4)
					{
					continue;
				}
				if (!arguments[0].Contains("dir"))
					{
					continue;
				}

				Vector3 vector = Vector3.zero;
				float.TryParse(arguments[1], out vector.x);
				float.TryParse(arguments[2], out vector.y);
				float.TryParse(arguments[3], out vector.z);
				return vector;
			}

			return Vector3.zero;
		}

		private Transform FindCarDriver(Transform parent)
		{
			for (int childIndex = 0; childIndex < parent.childCount; childIndex++)
			{
				Transform transform = parent.GetChild(childIndex);

				// MODIFIED: Default was to return the following as a result directly
				// return transform.gameObject.name.ToLower().Contains("driverposition") ? transform : FindCarDriver(transform);

				Transform driver = transform.gameObject.name.IndexOf("driverposition", StringComparison.InvariantCultureIgnoreCase) >= 0 ? transform : FindCarDriver(transform);

				if (driver != null)
				{
					return driver;
				}
			}

			return null;
		}

		private void PlaceCarWheelsVisuals(CarVisuals visual, GameObject car)
		{
			for (int childIndex = 0; childIndex < car.transform.childCount; childIndex++)
			{
				GameObject childObject = car.transform.GetChild(childIndex).gameObject;
				string name = childObject.name.ToLower();
				if (name.Contains("wheel"))
				{
					CarWheelVisuals comp = childObject.AddComponent<CarWheelVisuals>();
					foreach (MeshRenderer renderer in childObject.GetComponentsInChildren<MeshRenderer>())
					{
						if (renderer.gameObject.name.IndexOf("tire", StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							comp.tire_ = renderer;
							break;
						}
					}

					if (name.Contains("front"))
					{
						if (name.Contains("left"))
						{
							visual.wheelFL_ = comp;
						}
						else if (name.Contains("right"))
						{
							visual.wheelFR_ = comp;
						}
					}
					else if (name.Contains("back"))
					{
						if (name.Contains("left"))
						{
							visual.wheelBL_ = comp;
						}
						else if (name.Contains("right"))
						{
							visual.wheelBR_ = comp;
						}
					}
				}
			}
		}

		private CarColors LoadDefaultColors(GameObject car)
		{
			for (int carChildIndex = 0; carChildIndex < car.transform.childCount; carChildIndex++)
			{
				GameObject subObject = car.transform.GetChild(carChildIndex).gameObject;
				string subObjectName = subObject.name.ToLower();

				if (subObjectName.Contains("defaultcolor"))
				{
					for (int childIndex = 0; childIndex < subObject.transform.childCount; childIndex++)
					{
						GameObject subSubObject = subObject.transform.GetChild(childIndex).gameObject;
						string name = subSubObject.name.ToLower();

						if (!name.StartsWith("#"))
							{
							continue;
						}
						name = name.Remove(0, 1); //remove #

						string[] arguments = name.Split(';');
						if (arguments.Length != 2)
						{
							continue;
						}

						CarColors colors;
						Color color = ColorEx.HexToColor(arguments[1]);
						color.a = 1;
						if (arguments[0] == "primary")
						{
							colors.primary_ = color;
						}
						else if (arguments[0] == "secondary")
						{
							colors.secondary_ = color;
						}
						else if (arguments[0] == "glow")
						{
							colors.glow_ = color;
						}
						else if (arguments[0] == "sparkle")
						{
							colors.sparkle_ = color;
						}
					}
				}
			}

			return infos_.defaultColors;
		}
	}
}