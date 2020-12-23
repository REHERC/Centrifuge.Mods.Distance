using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.SceneDumper
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public string DumpSceneBasic
		{
			get => Get<string>("DumpSceneBasic");
			set => Set("DumpSceneBasic", value);
		}

		public string DumpSceneDetailed
		{
			get => Get<string>("DumpSceneDetailed");
			set => Set("DumpSceneDetailed", value);
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

			Get("DumpSceneBasic", "LeftControl+F7");
			Get("DumpSceneDetailed", "LeftControl+F8");

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
