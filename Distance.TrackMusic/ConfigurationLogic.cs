using Reactor.API.Configuration;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.TrackMusic
{
    public class ConfigurationLogic : MonoBehaviour
    {
        private Settings Config;

        public void Awake()
        {
            Config = new Settings("Config");

            foreach (var entry in new Dictionary<string, object>()
            {
                { "MaxMusicDownloadSizeMB", 30.0f },
                { "MaxMusicDownloadTimeSeconds", 15.0f },
                { "MaxMusicLevelLoadTimeSeconds", 20.0f }
            })
            {
                if (!Config.ContainsKey(entry.Key))
                {
                    Config[entry.Key] = entry.Value;
                }
            }
        }

        public void Update()
        {
            if (Config.Dirty)
            {
                Config.Save();
            }
        }

        #region Properties
        #region MaxMusicDownloadSize
        public float MaxMusicDownloadSizeMB
        {
            get => (float)Config["MaxMusicDownloadSizeMB"];
            set => Config["MaxMusicDownloadSizeMB"] = value;
        }

        public float MaxMusicDownloadSizeBytes
        {
            get => MaxMusicDownloadSizeMB * 1000 * 1000;
            set => MaxMusicDownloadSizeMB = value / 1000 / 1000;
        }
        #endregion
        #region MaxMusicDownloadTime
        public float MaxMusicDownloadTimeSeconds
        {
            get => (float)Config["MaxMusicDownloadTimeSeconds"];
            set => Config["MaxMusicDownloadTimeSeconds"] = value;
        }

        public float MaxMusicDownloadTimeMilli
        {
            get => MaxMusicDownloadTimeSeconds * 1000;
            set => MaxMusicDownloadTimeSeconds = value / 1000;
        }
        #endregion
        #region MaxMusicLevelLoadTime
        public float MaxMusicLevelLoadTimeSeconds
        {
            get => (float)Config["MaxMusicLevelLoadTimeSeconds"];
            set => Config["MaxMusicLevelLoadTimeSeconds"] = value;
        }

        public float MaxMusicLevelLoadTimeMilli
        {
            get => MaxMusicLevelLoadTimeSeconds * 1000;
            set => MaxMusicLevelLoadTimeSeconds = value / 1000;
        }
        #endregion
        #endregion
    }
}
