using System.Collections.Generic;
using System.Reflection;
using Reactor.API.Configuration;
using UnityEngine;

namespace CustomCar
{
    public static class ModdedCarsColors
    {
        public static void LoadAll()
        {
            var profileManager = G.Sys.ProfileManager_;

            if (profileManager == null)
                return;

            var settings = new Settings("CustomCars");

            var profiles =
                profileManager.GetType().GetField("profiles_", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(profileManager) as List<Profile>;

            foreach (var profile in profiles)
            {
                var colors =
                    profile.GetType().GetField("carColorsList_", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(profile) as CarColors[];

                var carInfosLength = G.Sys.ProfileManager_.carInfos_.Length;
                if (carInfosLength > colors.Length)
                {
                    var colors2 = new CarColors[carInfosLength];
                    for (var i = 0; i < colors.Length; i++)
                        colors2[i] = colors[i];
                    colors = colors2;
                }

                Section[] sProfile = null;
                try
                {
                    sProfile = settings.GetItem<Section[]>(profile.Name_);
                }
                catch
                {
                }

                if (sProfile == null)
                    continue;

                for (var i = 0; i < sProfile.Length; i++)
                    LoadCarColors(sProfile[i], ref colors[i + CustomCarsPatchInfos.baseCarCount]);

                profile.GetType().GetField("carColorsList_", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(profile, colors);
            }
        }

        private static void LoadCarColors(Section settings, ref CarColors colors)
        {
            try
            {
                LoadColor(settings.GetItem<Section>("primary"), ref colors.primary_);
            }
            catch
            {
            }

            try
            {
                LoadColor(settings.GetItem<Section>("secondary"), ref colors.secondary_);
            }
            catch
            {
            }

            try
            {
                LoadColor(settings.GetItem<Section>("glow"), ref colors.glow_);
            }
            catch
            {
            }

            try
            {
                LoadColor(settings.GetItem<Section>("sparkle"), ref colors.sparkle_);
            }
            catch
            {
            }
        }

        private static void LoadColor(Section settings, ref Color color)
        {
            if (settings == null)
                return;

            color.r = settings.GetOrCreate("r", color.r);
            color.g = settings.GetOrCreate("g", color.g);
            color.b = settings.GetOrCreate("b", color.b);
            color.a = settings.GetOrCreate("a", color.a);
        }

        public static void SaveAll()
        {
            var profileManager = G.Sys.ProfileManager_;

            if (profileManager == null)
                return;

            var settings = new Settings("CustomCars");
            settings.Clear();

            var profiles =
                profileManager.GetType().GetField("profiles_", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(profileManager) as List<Profile>;

            foreach (var profile in profiles)
            {
                var colors =
                    profile.GetType().GetField("carColorsList_", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(profile) as CarColors[];

                if (colors.Length <= CustomCarsPatchInfos.baseCarCount)
                    continue;

                var sProfile = new Section[colors.Length - CustomCarsPatchInfos.baseCarCount];
                settings[profile.Name_] = sProfile;

                for (var i = 0; i < sProfile.Length; i++)
                {
                    sProfile[i] = new Section();
                    SaveCarsColors(sProfile[i], colors[i + CustomCarsPatchInfos.baseCarCount]);
                }
            }

            settings.Save();
        }

        private static void SaveCarsColors(Section settings, CarColors colors)
        {
            var sPrimary = new Section();
            SaveColor(sPrimary, colors.primary_);
            var sSecondary = new Section();
            SaveColor(sSecondary, colors.secondary_);
            var sGlow = new Section();
            SaveColor(sGlow, colors.glow_);
            var sSparkle = new Section();
            SaveColor(sSparkle, colors.sparkle_);

            settings["primary"] = sPrimary;
            settings["secondary"] = sSecondary;
            settings["glow"] = sGlow;
            settings["sparkle"] = sSparkle;
        }

        private static void SaveColor(Section settings, Color color)
        {
            settings.Add("r", color.r);
            settings.Add("g", color.g);
            settings.Add("b", color.b);
            settings.Add("a", color.a);
        }
    }
}