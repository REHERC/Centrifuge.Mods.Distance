using Centrifuge.Distance.Game;
using Distance.NitronicHUD.Data;
using HarmonyLib;
using Reactor.API.Storage;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Distance.NitronicHUD.Scripts
{
    public class VisualDisplay : MonoBehaviour
    {
        public const string AssetName = "assets/nr hud/nr_hud.prefab";

        public Assets Assets { get; internal set; }

        public AssetBundle Bundle => Assets.Bundle as AssetBundle;

        private GameObject Prefab { get; set; }

        private VisualDisplayContent[] huds_;

        private Text timer_;

        public void Awake()
        {
            CreatePrefab();
        }
        public void ResetPrefab()
        {
            timer_ = null;
            huds_ = new VisualDisplayContent[0];

            Prefab.transform.parent = null;
            Prefab.SetActive(false);

            DestroyImmediate(Prefab);

            CreatePrefab(false);
        }

        public void ApplyTransformationInfo()
        {
            if (huds_.Length >= 2)
            {
                for (int x = 0; x <= 1; x++)
                {
                    float direction = x == 0 ? 1 : -1;

                    float scale = 0.5f; // whatever you want honestly...
                    float position = 0;

                    const float defaultScale = 1.7f;
                    float newScale = defaultScale * scale;

                    huds_[x].rectTransform.localScale = new Vector3(newScale * direction, newScale, newScale);
                    //huds_[x].rectTransform.position = new Vector3(position * direction, 0, 0);
                    huds_[x].rectTransform.anchoredPosition = new Vector2(position * direction, 0);

                    Mod.Instance.Logger.Warning($"{huds_[x].rectTransform.name} {direction}");

                }
            }

            if (timer_)
            {
                const float defaultScale = 0.5f;
                float scale = 0.5f; // whatever you want honestly...

                RectTransform rect = timer_.GetComponent<RectTransform>();

                if (rect)
                {
                    rect.localScale = Vector2.one * defaultScale * scale;
                }
            }
        }

        #region Initialize
        private void CreatePrefab(bool loadBundle = true)
        {
            if (loadBundle)
            {
                Assets = new Assets("hud.assets");
            }

            if (!Bundle)
            {
                Mod.Instance.Logger.Error($"The following assets file could not be loaded: hud.assets");

                DestroyImmediate(this);
                return;
            }

            Prefab = Instantiate(Bundle.LoadAsset<GameObject>(AssetName), transform);

            if (!Prefab)
            {
                Mod.Instance.Logger.Error($"The following asset from the hud.assets could not be loaded: \"{AssetName}\"");

                DestroyImmediate(this);
                return;
            }

            Prefab.name = "Visual Display";

            GameObject hud_left = Prefab.transform.Find("Hud_Left").gameObject;
            GameObject hud_right = Prefab.transform.Find("Hud_Right").gameObject;

            huds_ = new VisualDisplayContent[2]
            {
                new VisualDisplayContent(hud_left),
                new VisualDisplayContent(hud_right)
            };

            timer_ = Prefab?.transform.Find("Time")?.GetComponent<Text>();

            ApplyTransformationInfo();
        }
        #endregion
    
        public void Update()
        {
            if (huds_.Length == 0)
            {
                return;
            }

            UpdateTimerText();
            UpdateHeatIndicators();
        }

        #region Update Logic
        #region Overheat Meter
        // TODO: Add config for this
        private const float HeatBlinkStartAmount = 0.7f;
        private const float HeatBlinkFrequence = 2.0f;
        private const float HeatBlinkFrequenceBoost = 1.15f;
        private const float HeatBlinkAmount = 0.7f;
        private const float HeatFlameAmount = 0.5f;
        
        private void UpdateHeatIndicators()
        {
            try
            {
                float heat = Mathf.Clamp(Vehicle.HeatLevel, 0, 1);

                foreach (VisualDisplayContent instance in huds_)
                {
                    instance.heatHigh.fillAmount = heat;
                    instance.heatLow.fillAmount = heat;

                    float blink = 0;

                    if (heat > HeatBlinkStartAmount)
                    {
                        blink = (heat - HeatBlinkStartAmount) / (1 - HeatBlinkStartAmount);
                    }

                    blink *= 0.5f * Mathf.Sin((float)Timex.ModeTime_ * (HeatBlinkFrequence - ((1 - heat) * heat * HeatBlinkFrequenceBoost)) * 3 * Mathf.PI) + 0.5f;

                    instance.main.color = new Color(1, 1 - (blink * HeatBlinkAmount), 1 - (blink * HeatBlinkAmount));


                    float flame = 0;

                    if (heat > HeatFlameAmount)
                    {
                        flame = (heat - HeatFlameAmount) / (1 - HeatFlameAmount);
                    }

                    instance.flame.color = new Color(1, 1, 1, flame);
                }
            }
            catch (Exception ex)
            {
                Mod.Instance.Logger.Exception(ex);
            }
        }
        #endregion
        #region Timer Logic
        private void UpdateTimerText()
        {
            GameMode gamemode = G.Sys.GameManager_.Mode_;
            
            if (!gamemode || !timer_)
            {
                return;
            }

            float time = Mathf.Max(0, (float)gamemode.GetDisplayTime(0));

            StringBuilder result = new StringBuilder();

            GUtils.GetFormattedTime(result, time, 2, time > 3600);

            timer_.text = result.ToString();
        }
        #endregion
        #endregion
    }
}
