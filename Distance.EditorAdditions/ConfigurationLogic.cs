using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.EditorAdditions
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public float EditorIconSize
		{
			get => Get<float>("EditorIconSize");
			set => Set("EditorIconSize", value);
		}

		public bool DevFolderEnabled
		{
			get => Get<bool>("DevFolderEnabled");
			set => Set("DevFolderEnabled", value);
		}

		public bool AdvancedMusicSelection
		{
			get => Get<bool>("AdvancedMusicSelection");
			set => Set("AdvancedMusicSelection", value);
		}

		public bool DisplayWorkshopLevels
		{
			get => Get<bool>("DisplayWorkshopLevels");
			set => Set("DisplayWorkshopLevels", value);
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

			Get("EditorIconSize", 67f);
			Get("DevFolderEnabled", true);
			Get("AdvancedMusicSelection", true);
			Get("DisplayWorkshopLevels", true);

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
