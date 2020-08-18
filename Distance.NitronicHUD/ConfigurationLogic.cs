using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.NitronicHUD
{
    public class ConfigurationLogic : MonoBehaviour
    {
        #region Properties
        public bool ShowBars
        {
            get => Get<bool>("ShowBars");
            set => Set("ShowBars", value);
        }

        public bool ShowCountdown
        {
            get => Get<bool>("ShowCountdown");
            set => Set("ShowCountdown", value);
        }
        
        public bool AnnouncerCountdown
        {
            get => Get<bool>("AnnouncerCountdown");
            set => Set("AnnouncerCountdown", value);
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

            Get("ShowBars", true);
            Get("ShowCountdown", true);
            Get("AnnouncerCountdown", true);

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
