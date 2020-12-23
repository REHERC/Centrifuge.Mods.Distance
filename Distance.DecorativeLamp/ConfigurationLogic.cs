using Distance.DecorativeLamp.Enums;
using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.DecorativeLamp
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public bool Enabled
		{
			get => Get<bool>("Enabled");
			set => Set("Enabled", value);
		}

		public bool Spin
		{
			get => Get<bool>("Spin");
			set => Set("Spin", value);
		}

		public int SpinSpeed
		{
			get => Get<int>("SpinSpeed");
			set => Set("SpinSpeed", value);
		}

		public float LampScale
		{
			get => Get<float>("LampScale");
			set => Set("LampScale", value);
		}

		public float LightIntensity
		{
			get => Get<float>("LightIntensity");
			set => Set("LightIntensity", value);
		}

		public int LightRange
		{
			get => Get<int>("LightRange");
			set => Set("LightRange", value);
		}

		public float FlareBrightness
		{
			get => Get<float>("FlareBrightness");
			set => Set("FlareBrightness", value);
		}

		public LampModel MeshModel
		{
			get => (LampModel)Get<int>("MeshModel");
			set => Set("MeshModel", (int)value);
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

			Get("Enabled", true);
			Get("Spin", false);
			Get("SpinSpeed", 50);
			Get("LampScale", 1.0f);
			Get("LightIntensity", 0.75f);
			Get("LightRange", 20);
			Get("FlareBrightness", 1.0f);
			Get("MeshModel", LampModel.EmpireLamp);

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
