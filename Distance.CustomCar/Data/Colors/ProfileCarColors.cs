﻿using System.Collections.Generic;

namespace Distance.CustomCar.Data.Colors
{
	public class ProfileCarColors
	{
		private readonly ProfileDataLogic profileData_;

		public ProfileCarColors(ProfileDataLogic profileData)
		{
			profileData_ = profileData;
		}

		public void LoadColors()
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
					CarColors colors = profileData_.GetCarColors(currentProfile.FileName_, carInfo.name_);

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

					Mod.Instance.ProfileData.SetCarColors(currentProfile.FileName_, carInfo.name_, colors);
				}
			}
		}
	}
}