using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.NitronicHUD
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		#region System Active States
		public bool DisplayHeatMeters
		{
			get => Get<bool>("DisplayHeatMeters");
			set => Set("DisplayHeatMeters", value);
		}

		public bool DisplayCountdown
		{
			get => Get<bool>("DisplayCountdown");
			set => Set("DisplayCountdown", value);
		}

		public bool DisplayTimer
		{
			get => Get<bool>("DisplayTimer");
			set => Set("DisplayTimer", value);
		}

		public bool AnnouncerCountdown
		{
			get => Get<bool>("AnnouncerCountdown");
			set => Set("AnnouncerCountdown", value);
		}
		#endregion
		#region Systems Properties
		#region HUD
		public float HeatMetersScale
		{
			get => Get<float>("HeatMetersScale");
			set => Set("HeatMetersScale", value);
		}

		public int HeatMetersHorizontalOffset
		{
			get => Get<int>("HeatMetersHorizontalOffset");
			set => Set("HeatMetersHorizontalOffset", value);
		}

		public int HeatMetersVerticalOffset
		{
			get => Get<int>("HeatMetersVerticalOffset");
			set => Set("HeatMetersVerticalOffset", value);
		}

		public float TimerScale
		{
			get => Get<float>("TimerScale");
			set => Set("TimerScale", value);
		}

		public int TimerVerticalOffset
		{
			get => Get<int>("TimerVerticalOffset");
			set => Set("TimerVerticalOffset", value);
		}

		public float HeatBlinkStartAmount
		{
			get => Get<float>("HeatBlinkStartAmount");
			set => Set("HeatBlinkStartAmount", value);
		}

		public float HeatBlinkFrequence
		{
			get => Get<float>("HeatBlinkFrequence");
			set => Set("HeatBlinkFrequence", value);
		}

		public float HeatBlinkFrequenceBoost
		{
			get => Get<float>("HeatBlinkFrequenceBoost");
			set => Set("HeatBlinkFrequenceBoost", value);
		}

		public float HeatBlinkAmount
		{
			get => Get<float>("HeatBlinkAmount");
			set => Set("HeatBlinkAmount", value);
		}

		public float HeatFlameAmount
		{
			get => Get<float>("HeatFlameAmount");
			set => Set("HeatFlameAmount", value);
		}
		#endregion
		#endregion
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

			Get("DisplayHeatMeters", true);
			Get("DisplayCountdown", true);
			Get("DisplayTimer", true);
			Get("AnnouncerCountdown", true);

			Get("HeatMetersScale", 1.0f);
			Get("HeatMetersHorizontalOffset", 0);
			Get("HeatMetersVerticalOffset", 0);
			Get("TimerScale", 1.0f);
			Get("TimerVerticalOffset", 0);

			Get("HeatBlinkStartAmount", 0.7f);
			Get("HeatBlinkFrequence", 2.0f);
			Get("HeatBlinkFrequenceBoost", 1.15f);
			Get("HeatBlinkAmount", 0.7f);
			Get("HeatFlameAmount", 0.5f);

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
