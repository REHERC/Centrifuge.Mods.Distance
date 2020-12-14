using Distance.CustomCar.Data.Materials;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public class CarBuilder : List<CarData>
	{
		private readonly CarFactory factory_;

		public CarBuilder(CarFactory factory)
		{
			factory_ = factory;
		}

		public void CreateCars()
		{
			CarInfos infos = factory_.Infos;

			foreach (GameObject carPrefab in factory_.Prefabs)
			{
				Add(CreateCar(carPrefab));
			}
		}

		private CarData CreateCar(GameObject car)
		{
			GameObject carBase = Object.Instantiate(factory_.Infos.baseCar);
			Object.DontDestroyOnLoad(carBase);
			carBase.name = car.name;
			carBase.SetActive(false);

			RemoveDefaultCarObjects(carBase);
			GameObject carSkin = Object.Instantiate(car, carBase.transform);
			AddCarComponents(carBase, carSkin);

			// return line is just to not get compile error
			return new CarData(carBase, G.Sys.ProfileManager_.CarInfos_[0].colors_);
		}

		private void RemoveDefaultCarObjects(GameObject carPrefab)
		{
			List<GameObject> wheelsToRemove = new List<GameObject>();

			foreach (GameObject childObject in carPrefab.GetChildren())
			{
				if (childObject.name.ToLower().Contains("wheel"))
				{
					wheelsToRemove.Add(childObject);
				}
			}

			if (wheelsToRemove.Count != 4)
			{
				factory_.Errors.Add($"Found {wheelsToRemove.Count} wheels on base prefab, expected 4", "Game assets");
			}

			Transform refractorTransform = carPrefab.transform.Find("Refractor");

			if (refractorTransform)
			{
				refractorTransform.gameObject.DeactivateAndDestroy();
			}
			else
			{
				factory_.Errors.Add("Can't find the Refractor object on the base car prefab", "Game assets");
			}

			wheelsToRemove.ForEach((wheel) => wheel.DeactivateAndDestroy());
		}

		private void AddCarComponents(GameObject carBase, GameObject carSkin)
		{
			AddColorChanger(carBase.GetComponent<ColorChanger>(), carSkin);
		}

		private void AddColorChanger(ColorChanger colorChanger, GameObject carSkin)
		{
			if (colorChanger)
			{
				foreach (Renderer renderer in carSkin.GetComponentsInChildren<Renderer>())
				{
					ReplaceRendererMaterials(renderer);
				}
			}
			else
			{
				factory_.Errors.Add("Can't find the ColorChanger component on the base car", "Game assets");
			}
		}

		private void ReplaceRendererMaterials(Renderer renderer)
		{
			string[] materialNames = new string[renderer.materials.Length];
			List<MaterialProperty>[] materialProperties = new List<MaterialProperty>[renderer.materials.Length];

			for (int materialIndex = 0; materialIndex < materialNames.Length; materialIndex++)
			{
				materialNames[materialIndex] = "wheel";
				materialProperties[materialIndex] = new List<MaterialProperty>();
			}

			FillMaterialInfos(renderer, materialNames, materialProperties);

			Material[] materials = renderer.materials;

			for (int materialIndex = 0; materialIndex < materialNames.Length; materialIndex++)
			{
				if (!factory_.Infos.materials.TryGetValue(materialNames[materialIndex], out MaterialInfos materialInfos))
				{
					factory_.Errors.Add($"Can't find the material {materialNames[materialIndex]} on {renderer.gameObject.FullName()}", "Custom assets materials");
					continue;
				}

				if (materialInfos == null || !materialInfos.material)
				{
					continue;
				}

				Material material = Object.Instantiate(materialInfos.material);
				Material rendererMaterial = renderer.materials[materialIndex];
				if (materialInfos.diffuseID >= 0)
				{
					material.SetTexture(materialInfos.diffuseID, rendererMaterial.GetTexture("_MainTex"));
				}

				if (materialInfos.normalID >= 0)
				{
					material.SetTexture(materialInfos.normalID, rendererMaterial.GetTexture("_BumpMap"));
				}

				if (materialInfos.emitID >= 0)
				{
					material.SetTexture(materialInfos.emitID, rendererMaterial.GetTexture("_EmissionMap"));
				}

				foreach (MaterialProperty property in materialProperties[materialIndex])
				{
					CopyMaterialProperty(renderer.materials[materialIndex], material, property);
				}

				materials[materialIndex] = material;
			}

			renderer.materials = materials;
		}

		private void FillMaterialInfos(Renderer renderer, string[] names, List<MaterialProperty>[] properties)
		{
			int childCount = renderer.transform.childCount;
			for (int childIndex = 0; childIndex < childCount; childIndex++)
			{
				string name = renderer.transform.GetChild(childIndex).name;
				if (!name.StartsWith("#"))
				{
					continue;
				}
				name = name.Remove(0, 1);

				string[] materialArguments = name.Split(';');
				if (materialArguments.Length == 0)
				{
					continue;
				}

				string category = materialArguments[0].ToLower();

				if (category.Contains("mat"))
				{
					if (materialArguments.Length != 3)
					{
						factory_.Errors.Add($"{materialArguments[0]} property on {renderer.gameObject.FullName()} must have 2 arguments", "Custom assets materials");
						continue;
					}

					if (!int.TryParse(materialArguments[1], out int index))
					{
						factory_.Errors.Add($"First argument of {materialArguments[0]} on {renderer.gameObject.FullName()} property must be a number", "Custom assets materials");
						continue;
					}

					if (index < names.Length)
					{
						names[index] = materialArguments[2];
					}
				}
				else if (category.Contains("export"))
				{
					if (materialArguments.Length != 5)
					{
						factory_.Errors.Add($"{materialArguments[0]} property on {renderer.gameObject.FullName()} must have 4 arguments", "Custom assets materials");
						continue;
					}

					if (!int.TryParse(materialArguments[1], out int index))
					{
						factory_.Errors.Add($"First argument of {materialArguments[0]} on {renderer.gameObject.FullName()} property must be a number", "Custom assets materials");
						continue;
					}

					if (index >= names.Length)
					{
						names[index] = materialArguments[2];
					}

					MaterialProperty property = new MaterialProperty();
					bool materialTypeFound = false;

					foreach (PropertyType type in System.Enum.GetValues(typeof(PropertyType)))
					{
						if (string.Equals(materialArguments[2], type.ToString(), System.StringComparison.InvariantCultureIgnoreCase))
						{
							property.type = type;
							materialTypeFound = true;
							break;
						}
					}

					if (!materialTypeFound)
					{
						factory_.Errors.Add($"The property {materialArguments[2]} on {renderer.gameObject.FullName()} is not valid", "Custom assets materials");
						continue;
					}

					if (!int.TryParse(materialArguments[3], out property.fromID))
					{
						property.fromName = materialArguments[3];
					}

					if (!int.TryParse(materialArguments[4], out property.toID))
					{
						property.toName = materialArguments[4];
					}

					properties[index].Add(property);
				}
			}
		}
		
		private void CopyMaterialProperty(Material from, Material to, MaterialProperty property)
		{
			int fromID = property.fromID;
			if (fromID < 0)
			{
				fromID = Shader.PropertyToID(property.fromName);
			}

			int toID = property.toID;
			if (toID < 0)
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
	}
}