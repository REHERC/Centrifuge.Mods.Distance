using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.MenuUtilities
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public bool EnableDeletePlaylistButton
		{
			get => Get<bool>("EnableDeletePlaylistButton");
			set => Set("EnableDeletePlaylistButton", value);
		}

		public bool EnableHexColorInput
		{
			get => Get<bool>("EnableHexColorInput");
			set => Set("EnableHexColorInput", value);
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

			Get("EnableDeletePlaylistButton", true);
			Get("EnableHexColorInput", true);

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