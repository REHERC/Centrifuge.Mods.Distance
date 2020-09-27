using Distance.Heat.Enums;
using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.Heat
{
    public class ConfigurationLogic : MonoBehaviour
    {
        #region Properties
        // TODO: Add trigger hotkey (see scenedumper for reference)
        public string ToggleHotkey
        {
            get => Get<string>("ToggleHotkey");
            set => Set("ToggleHotkey", value);
        }

        public ActivationMode ActivationMode
        {
            get => (ActivationMode)Get<int>("ActivationMode");
            set => Set("ActivationMode", value);
        }

        public DisplayMode DisplayMode
        {
            get => (DisplayMode)Get<int>("DisplayMode");
            set => Set("DisplayMode", value);
        }

        public float WarningTreshold
        {
            get => Get<float>("WarningTreshold");
            set => Set("WarningTreshold", value);
        }
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

            Get("ToggleHotkey", "LeftControl+H");
            Get("ActivationMode", ActivationMode.Always);
            Get("DisplayMode", DisplayMode.Hud);
            Get("WarningThreshold", 0.8f);

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
