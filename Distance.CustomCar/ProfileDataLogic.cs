using Reactor.API.Configuration;
using System;
using UnityEngine;


namespace Distance.CustomCar
{
	public class ProfileDataLogic : MonoBehaviour
	{
		internal Settings Config;

		public event Action<ProfileDataLogic> OnChanged;

		private void Load()
		{
			Config = new Settings("CustomCars");
		}

		public void Awake()
		{
			Load();

			Save();
		}

		public Section Profile(string profileName)
		{
			Section profile = Config.GetOrCreate(profileName, new Section());
			Save();
			return profile;
		}

		public Section Vehicle(string profileName, string vehicleName)
		{
			Section vehicle = Profile(profileName).GetOrCreate(vehicleName, new Section());
			Save();
			return vehicle;
		}

		public CarColors GetCarColors(string profileName, string vehicleName)
		{
			Section vehicle = Vehicle(profileName, vehicleName);
			CarColors colors = new CarColors
			{
				primary_ = GetColor(vehicle, "primary", Colors.whiteSmoke),
				secondary_ = GetColor(vehicle, "primary", Colors.darkGray),
				glow_ = GetColor(vehicle, "glow", Colors.cyan),
				sparkle_ = GetColor(vehicle, "sparkle", Colors.lightSlateGray)
			};
			Save();
			return colors;
		}

		public Color GetColor(Section vehicle, string category, Color defaultColor)
		{
			Section color = vehicle.GetOrCreate(category, new Section());

			var r = color.GetOrCreate("r", defaultColor.r);
			var g = color.GetOrCreate("g", defaultColor.g);
			var b = color.GetOrCreate("b", defaultColor.b);
			var a = color.GetOrCreate("a", defaultColor.a);

			return new Color(r, g, b, a);
		}

		public void SetCarColors(string profileName, string vehicleName, CarColors colors)
		{
			Section vehicle = Vehicle(profileName, vehicleName);

			vehicle["primary"] = ToSection(colors.primary_);
			vehicle["secondary"] = ToSection(colors.secondary_);
			vehicle["glow"] = ToSection(colors.glow_);
			vehicle["sparkle"] = ToSection(colors.sparkle_);

			Save();
		}

		public Section ToSection(Color color)
		{
			Section section = new Section
			{
				["r"] = color.r,
				["g"] = color.g,
				["b"] = color.b,
				["a"] = color.a
			};
			Save();
			return section;
		}

		public void Save()
		{
			Config.Save();
			OnChanged?.Invoke(this); 
		}
	}
}