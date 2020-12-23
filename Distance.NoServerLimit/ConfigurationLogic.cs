using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.NoServerLimit
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public int MaxPlayerCount
		{
			get => Get<int>("MaxPlayerCount");
			set => Set("MaxPlayerCount", value);
		}
		#endregion

		internal Settings Config;

		public event Action<ConfigurationLogic> OnChanged;

		private void Load()
		{
			Config = new Settings("Server");
		}

		public void Awake()
		{
			Load();

			Get("MaxPlayerCount", 32);

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
