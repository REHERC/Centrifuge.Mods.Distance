#pragma warning disable IDE0059
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Distance.DecorativeLamp
{
    public class DecorativeLampLogic : MonoBehaviour
    {
        public const string LampPrefabName = "Prefabs/LevelEditor/Decorations/Empirelamp";

        private Material materialLampLight_;
        private Material materialLampPanel_;
        private Light light_;
        private LensFlareFadeWithDistance flare_;
        private CarColors carColors_;
        private CarLogic carLogic_;
        private GameObject lamp_;
        private string sceneName_ = string.Empty;
        private CustomizeCarColorsMenuLogic customizeColorsMenu_ = null;

        public void Start()
        {
            carLogic_ = GetComponentInParent<CarLogic>();

            lamp_ = Instantiate(Resources.Load<GameObject>(LampPrefabName), transform.position, transform.rotation);
            
            lamp_.transform.parent = transform.parent;

            foreach (Collider collider in lamp_.GetComponentsInChildren<MeshCollider>(true))
            {
                collider.Destroy();
            }

            flare_ = lamp_.GetComponentInChildren<LensFlareFadeWithDistance>();

            foreach (MeshRenderer renderer in lamp_.GetComponentsInChildren<MeshRenderer>())
            {
                foreach (Material material in renderer.materials)
                {
                    switch (material.name.ToLowerInvariant().Split(' ')[0])
                    {
                        case "empire_light_strip":
                            materialLampLight_ = material;
                            break;
                        case "empire_panel":
                            materialLampPanel_ = material;
                            break;
                    }
                }
            }

            light_ = lamp_.GetComponentInChildren<Light>();
            lamp_.SetActive(Mod.Instance.Config.Enabled);

            Events.Scene.LoadFinish.Subscribe(OnSceneLoaded);
        }
        public void OnDestroy()
        {
            Events.Scene.LoadFinish.Unsubscribe(OnSceneLoaded);
        }

        private void OnSceneLoaded(Events.Scene.LoadFinish.Data data)
        {
            sceneName_ = SceneManager.GetActiveScene().name;
            customizeColorsMenu_ = FindObjectOfType<CustomizeCarColorsMenuLogic>();
        }

        public void Update()
        {
            ConfigurationLogic config = Mod.Instance.Config;
            
            lamp_.transform.localScale = Vector3.one * config.LampScale * 0.1f;
            light_.intensity = config.LightIntensity;
            flare_.lensFlare_.brightness = config.FlareBrightness;
            flare_.Update();

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
            materialLampLight_.SetColor("_Emit", color);

            light_.color = color;
            flare_.lensFlare_.color = color;

            color = carColors_.primary_;
            color.a = 255;
            materialLampPanel_.SetColor("_Color", color);
            materialLampPanel_.SetColor("_SpecColor", color);
        }
    }
}
