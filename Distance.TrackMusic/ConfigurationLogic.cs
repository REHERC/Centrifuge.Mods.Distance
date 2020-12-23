using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.TrackMusic
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public float MaxMusicDownloadSizeMB
		{
			get => Get<float>("MaxMusicDownloadSizeMB");
			set => Set("MaxMusicDownloadSizeMB", value);
		}

		public float MaxMusicDownloadSizeBytes
		{
			get => MaxMusicDownloadSizeMB * 1000 * 1000;
			set => MaxMusicDownloadSizeMB = value / 1000 / 1000;
		}

		public float MaxMusicDownloadTimeSeconds
		{
			get => Get<float>("MaxMusicDownloadTimeSeconds");
			set => Set("MaxMusicDownloadTimeSeconds", value);
		}

		public float MaxMusicDownloadTimeMilli
		{
			get => MaxMusicDownloadTimeSeconds * 1000;
			set => MaxMusicDownloadTimeSeconds = value / 1000;
		}

		public float MaxMusicLevelLoadTimeSeconds
		{
			get => Get<float>("MaxMusicLevelLoadTimeSeconds");
			set => Set("MaxMusicLevelLoadTimeSeconds", value);
		}

		public float MaxMusicLevelLoadTimeMilli
		{
			get => MaxMusicLevelLoadTimeSeconds * 1000;
			set => MaxMusicLevelLoadTimeSeconds = value / 1000;
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

			Get("MaxMusicDownloadSizeMB", 30.0f);
			Get("MaxMusicDownloadTimeSeconds", 15.0f);
			Get("MaxMusicLevelLoadTimeSeconds", 20.0f);

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