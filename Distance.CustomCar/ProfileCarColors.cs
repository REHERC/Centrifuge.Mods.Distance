using Reactor.API.Configuration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.CustomCar
{
	public class ProfileCarColors : MonoBehaviour
	{
		internal Settings Config;

		public event Action<ProfileCarColors> OnChanged;

		protected void Load()
		{
			Config = new Settings("CustomCars");
		}

		protected void Awake()
		{
			Load();
			Save();
		}

		protected Section Profile(string profileName)
		{
			return Config.GetOrCreate(profileName, new Section());
		}

		protected Section Vehicle(string profileName, string vehicleName)
		{
			return Profile(profileName).GetOrCreate(vehicleName, new Section());
		}

		protected CarColors GetCarColors(string profileName, string vehicleName)
		{
			Section vehicle = Vehicle(profileName, vehicleName);
			CarColors colors = new CarColors
			{
				primary_ = GetColor(vehicle, "primary", Colors.whiteSmoke),
				secondary_ = GetColor(vehicle, "secondary", Colors.darkGray),
				glow_ = GetColor(vehicle, "glow", Colors.cyan),
				sparkle_ = GetColor(vehicle, "sparkle", Colors.lightSlateGray)
			};
			SetCarColors(profileName, vehicleName, colors);
			return colors;
		}

		protected Color GetColor(Section vehicle, string category, Color defaultColor)
		{
			Section color = vehicle.GetOrCreate(category, new Section());

			var r = color.GetOrCreate("r", defaultColor.r);
			var g = color.GetOrCreate("g", defaultColor.g);
			var b = color.GetOrCreate("b", defaultColor.b);
			var a = color.GetOrCreate("a", defaultColor.a);

			return new Color(r, g, b, a);
		}

		protected void SetCarColors(string profileName, string vehicleName, CarColors colors)
		{
			Section vehicle = Vehicle(profileName, vehicleName);

			vehicle["primary"] = ToSection(colors.primary_);
			vehicle["secondary"] = ToSection(colors.secondary_);
			vehicle["glow"] = ToSection(colors.glow_);
			vehicle["sparkle"] = ToSection(colors.sparkle_);

			Section profile = Profile(profileName);
			profile[vehicleName] = vehicle;

			Config[profileName] = profile;
		}

		protected Section ToSection(Color color)
		{
			Section section = new Section
			{
				["r"] = color.r,
				["g"] = color.g,
				["b"] = color.b,
				["a"] = color.a
			};
			return section;
		}

		protected void Save()
		{
			Config.Save();
			OnChanged?.Invoke(this);
		}

		public void LoadAll()
		{
			ProfileManager profileManager = G.Sys.ProfileManager_;
			List<Profile> allProfiles = profileManager.profiles_;

			foreach (Profile currentProfile in allProfiles)
			{
				CarColors[] carColors = new CarColors[Mod.TotalCarCount];

				for (int carIndex = 0; carIndex < profileManager.CarInfos_.Length; carIndex++)
				{
					if (carIndex < Mod.DefaultCarCount)
					{
						carColors[carIndex] = currentProfile.carColorsList_[carIndex];
						continue;
					}

					CarInfo carInfo = profileManager.CarInfos_[carIndex];
					CarColors colors = GetCarColors(currentProfile.FileName_, carInfo.name_);

					carColors[carIndex] = colors;
				}

				currentProfile.carColorsList_ = carColors;
			}
		}

		public void SaveAll()
		{
			ProfileManager profileManager = G.Sys.ProfileManager_;

			List<Profile> allProfiles = profileManager.profiles_;

			foreach (Profile currentProfile in allProfiles)
			{
				for (int carIndex = 0; carIndex < profileManager.CarInfos_.Length; carIndex++)
				{
					if (carIndex < Mod.DefaultCarCount)
					{
						continue;
					}

					CarInfo carInfo = profileManager.CarInfos_[carIndex];
					CarColors colors = currentProfile.carColorsList_[carIndex];

					SetCarColors(currentProfile.FileName_, carInfo.name_, colors);
				}
			}

			Save();
		}
	}
}
