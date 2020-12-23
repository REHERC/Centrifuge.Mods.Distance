using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.Anolytics
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public bool LogAnalytics
		{
			get => Get<bool>("LogAnalytics");
			set => Set("LogAnalytics", value);
		}

		public bool ShutDownAnalytics
		{
			get => Get<bool>("ShutDownAnalytics");
			set => Set("ShutDownAnalytics", value);
		}

		public bool DisablePlaytestingDataLogging
		{
			get => Get<bool>("DisablePlaytestingDataLogging");
			set => Set("DisablePlaytestingDataLogging", value);
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

			Get("LogAnalytics", true);
			Get("ShutDownAnalytics", true);
			Get("DisablePlaytestingDataLogging", true);

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
