using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.CustomCar
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public bool UseTrumpetHorn
		{
			get => Get<bool>("UseTrumpetHorn");
			set => Set("UseTrumpetHorn", value);
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

			Get("UseTrumpetHorn", false);

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