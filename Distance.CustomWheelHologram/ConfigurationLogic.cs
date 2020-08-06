using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.CustomWheelHologram
{
    public class ConfigurationLogic : MonoBehaviour
    {
        #region Properties
        public bool Enabled
        {
            get => Get<bool>("Enabled");
            set => Set("Enabled", value);
        }

        public string FileName
        {
            get => Get<string>("FileName");
            set => Set("FileName", value);
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

            Get("Enabled", false);
            Get("FileName", string.Empty);

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
