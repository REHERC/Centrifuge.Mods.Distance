using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.ChristmasSeasonalFeatures
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public ActivationMode ActivationMode
		{
			get => (ActivationMode)Get<byte>("ActivationMode");
			set => Set("ActivationMode", (byte)value);
		}

		public TimeFormat TimeFormat
		{
			get => (TimeFormat)Get<byte>("TimeFormat");
			set => Set("TimeFormat", (byte)value);
		}

		public bool OverrideLoadingScreens
		{
			get => Get<bool>("OverrideLoadingScreens");
			set => Set("OverrideLoadingScreens", value);
		}

		public bool ChristmasSnowVisualCheat
		{
			get => Get<bool>("ChristmasSnowVisualCheat");
			set => Set("ChristmasSnowVisualCheat", value);
		}

		public bool ReindeerCosmeticVisualCheat
		{
			get => Get<bool>("ReindeerCosmeticVisualCheat");
			set => Set("ReindeerCosmeticVisualCheat", value);
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

			Get("ActivationMode", ActivationMode.DuringDecember);
			Get("TimeFormat", TimeFormat.Local);
			Get("OverrideLoadingScreens", true);
			Get("ChristmasSnowVisualCheat", true);
			Get("ReindeerCosmeticVisualCheat", true);

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
