using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.DecorativeLamp
{
    public class ConfigurationLogic : MonoBehaviour
    {
        #region Properties
        public bool Enabled
        {
            get => Get<bool>("Enabled");
            set => Set("Enabled", value);
        }

        public bool Spin
        {
            get => Get<bool>("Spin");
            set => Set("Spin", value);
        }

        public int SpinSpeed
        {
            get => Get<int>("SpinSpeed");
            set => Set("SpinSpeed", value);
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

            Get("Enabled", true);
            Get("Spin", false);
            Get("SpinSpeed", 50);

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
