using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.NitronicHUD
{
    public class ConfigurationLogic : MonoBehaviour
    {
        #region Properties
        #region System Active States
        public bool DisplayHeatMeters
        {
            get => Get<bool>("DisplayHeatMeters");
            set => Set("DisplayHeatMeters", value);
        }

        public bool DisplayCountdown
        {
            get => Get<bool>("DisplayCountdown");
            set => Set("DisplayCountdown", value);
        }

        public bool DisplayTimer
        {
            get => Get<bool>("DisplayTimer");
            set => Set("DisplayTimer", value);
        }

        public bool AnnouncerCountdown
        {
            get => Get<bool>("AnnouncerCountdown");
            set => Set("AnnouncerCountdown", value);
        }
        #endregion
        #region Systems Properties
        #region HUD
        public float HeatMetersScale
        {
            get => Get<float>("HeatMetersScale");
            set => Set("HeatMetersScale", value);
        }

        public float HeatMetersOffset
        {
            get => Get<float>("HeatMetersOffset");
            set => Set("HeatMetersOffset", value);
        }

        public float TimerScale
        {
            get => Get<float>("TimerScale");
            set => Set("TimerScale", value);
        }
        #endregion
        #endregion
        #endregion

        internal Settings Config;

        public event Action<ConfigurationLogic> OnChanged;

        private void Load() 
        {
            Config = new Settings("Config");
        }

        public void Awake()
        {
            Load();

            Get("DisplayHeatMeters", true);
            Get("DisplayCountdown", true);
            Get("DisplayTimer", true);
            Get("AnnouncerCountdown", true);

            Get("HeatMetersScale", 1.0f);
            Get("HeatMetersOffset", 0.0f);
            Get("TimerScale", 1.0f);

            Save();
        }

        public T Get<T>(string key, T @default = default)
        {
            return Config.GetOrCreate(key, @default);
        }

        public void Set<T>(string key, T value)
        {
            Config[key] = value;
            Save();
        }

        public void Save()
        {
            Config?.Save();
            OnChanged?.Invoke(this);
        }
    }
}
