#pragma warning disable IDE0059
using Distance.DecorativeLamp.Enums;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Distance.DecorativeLamp
{
	public class DecorativeLampLogic : MonoBehaviour
	{
		public const string EmpireLampPrefab = "Prefabs/LevelEditor/Decorations/EmpireLamp";
		public const string EmpireLampAlternatePrefab = "Prefabs/LevelEditor/Decorations/EmpireLamp02";
		public const string NitronicLampPrefab = "Prefabs/LevelEditor/Decorations/Echoes/NitronicLamp";

		private Material materialLampLight_;
		private Material materialLampPanel_;
		private Light light_;
		private LensFlareLogic flare_;
		private LensFlare[] flares_;
		private CarColors carColors_;
		private CarLogic carLogic_;
		private GameObject lamp_;
		private string sceneName_ = string.Empty;
		private CustomizeCarColorsMenuLogic customizeColorsMenu_ = null;

		#region Prefab Managment
		public void CreateLamp()
		{
			string model = "";

			switch (Mod.Instance.Config.MeshModel)
			{
				case LampModel.EmpireLamp:
					model = EmpireLampPrefab;
					break;
				case LampModel.EmpirePilar:
					model = EmpireLampAlternatePrefab;
					break;
				case LampModel.NitronicLamp:
					model = NitronicLampPrefab;
					break;
			}

			carLogic_ = GetComponentInParent<CarLogic>();

			lamp_ = Instantiate(Resources.Load<GameObject>(model), transform.position, transform.rotation);

			lamp_.transform.parent = transform.parent;

			foreach (Collider collider in lamp_.GetComponentsInChildren<MeshCollider>(true))
			{
				collider.Destroy();
			}

			flare_ = lamp_.GetComponentInChildren<LensFlareLogic>();
			flares_ = lamp_.GetComponentsInChildren<LensFlare>() ?? new LensFlare[0];

			foreach (MeshRenderer renderer in lamp_.GetComponentsInChildren<MeshRenderer>())
			{
				foreach (Material material in renderer.materials)
				{
					switch (material.name.ToLowerInvariant().Split(' ')[0])
					{
						case "empire_light_strip":
						case "nitronic_lamplight":
							materialLampLight_ = material;
							break;
						case "empire_panel":
						case "nitronicpanel":
							materialLampPanel_ = material;
							break;
					}
				}
			}

			light_ = lamp_.GetComponentInChildren<Light>();
			lamp_.SetActive(Mod.Instance.Config.Enabled);
		}

		public void ResetLamp()
		{
			lamp_?.Destroy();

			CreateLamp();
		}
		#endregion

		#region Event Subscribers
		private void OnSceneLoaded(Events.Scene.LoadFinish.Data data)
		{
			sceneName_ = SceneManager.GetActiveScene().name;
			customizeColorsMenu_ = FindObjectOfType<CustomizeCarColorsMenuLogic>();
		}

		private void OnModelChanged(Events.DecorativeLamp.ChangeLampModel.Data data)
		{
			ResetLamp();
		}
		#endregion

		#region Unity Messages
		public void Start()
		{
			CreateLamp();

			Events.Scene.LoadFinish.Subscribe(OnSceneLoaded);
			Events.DecorativeLamp.ChangeLampModel.Subscribe(OnModelChanged);
		}

		public void OnDestroy()
		{
			Events.Scene.LoadFinish.Unsubscribe(OnSceneLoaded);
			Events.DecorativeLamp.ChangeLampModel.Unsubscribe(OnModelChanged);
		}

		public void Update()
		{
			if (!lamp_ || !flare_ || !light_)
			{
				return;
			}

			try
			{
				ConfigurationLogic config = Mod.Instance.Config;

				lamp_.transform.localScale = Vector3.one * config.LampScale * 0.1f;
				light_.intensity = config.LightIntensity;

				if (flare_)
				{
					flare_.lensFlare_.brightness = config.FlareBrightness;
				}

				foreach (LensFlare flare in flares_)
				{
					flare.brightness = config.FlareBrightness;
				}

				light_.range = config.LightRange;

				if (Mod.Instance.Config.Spin)
				{
					lamp_.transform.RotateAround(lamp_.transform.position, lamp_.transform.up, Mod.Instance.Config.SpinSpeed * Timex.deltaTime_);
				}
				else
				{
					Vector3 rotation = lamp_.transform.localRotation.eulerAngles;

					rotation.y = 0;

					lamp_.transform.localRotation = Quaternion.Euler(rotation);
				}
			}
			catch
			{
				// First frame after Start always throws a NullReferenceException so just ignore it
			}
		}

		public void LateUpdate()
		{
			lamp_?.SetActive(Mod.Instance.Config.Enabled);

			if (!Mod.Instance.Config.Enabled)
			{
				return;
			}

			carColors_ = G.Sys.ProfileManager_.CurrentProfile_.CarColors_;

			// If color picker is open take input as colors, otherwise select profile car colors
			if (customizeColorsMenu_ && (customizeColorsMenu_.editColorsPanel_.IsTop_ || customizeColorsMenu_.colorPickerPanel_.IsTop_ || customizeColorsMenu_.colorPresetPanel_.IsTop_))
			{
				carColors_ = customizeColorsMenu_.colorPickerPanel_.IsTop_ ? customizeColorsMenu_.pickerCarColor_.Colors_ : customizeColorsMenu_.CurrentCarColor_.Colors_;
			}
			else if (!string.Equals(sceneName_, "MainMenu", StringComparison.InvariantCultureIgnoreCase) && carLogic_)
			{
				carColors_ = carLogic_.PlayerData_.originalColors_.Colors_;
			}

			Color color = Color.white;

			color = carColors_.glow_;
			color.a = 255;
			materialLampLight_.SetColor("_Color", color);
			materialLampLight_.SetColor("_Emit", color);
			materialLampPanel_.SetColor("_EmitColor", color);

			light_.color = color;

			if (flare_)
			{
				flare_.lensFlare_.color = color;
			}

			foreach (LensFlare flare in flares_)
			{
				flare.color = color;
			}

			color = carColors_.primary_;
			color.a = 255;
			materialLampPanel_.SetColor("_Color", color);
			materialLampPanel_.SetColor("_SpecColor", color);
		}
		#endregion
	}
}
