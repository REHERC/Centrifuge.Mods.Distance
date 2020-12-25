using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Distance.ChristmasSeasonalFeatures
{
	[ModEntryPoint("com.github.reherc/Distance.ChristmasSeasonalFeatures")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public bool IsActive => ModActivationLogic.IsModActive();

		public IManager Manager { get; set; }

		public FileSystem Data { get; set; }

		public Log Logger { get; set; }

		public ConfigurationLogic Config { get; private set; }

		public LoadingScreenTextureLogic LoadingScreenTextures { get; private set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;

			Data = new FileSystem();
			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigurationLogic>();
			LoadingScreenTextures = gameObject.AddComponent<LoadingScreenTextureLogic>();

			RuntimePatcher.AutoPatch();
			CreateSettingsMenu();
		}

		public void Load()
		{
			string containingFolder = Path.Combine(Data.VirtualFileSystemRoot, InternalResources.Constants.LOADINGSCREENS_FOLDER);

			foreach (string fileName in InternalResources.Constants.LOADINGSCREENS_FILES)
			{
				FileInfo textureFileInfo = new FileInfo(Path.Combine(containingFolder, fileName));

				if (textureFileInfo.Exists)
				{
					LoadingScreenTextures.AddTexture(textureFileInfo);
				}
				else
				{
					Logger.Error($"Missing file: {textureFileInfo.FullName}");
				}
			}

			Logger.Info($"{LoadingScreenTextures.Textures.Length} texture(s) loaded");
		}

		public void CreateSettingsMenu()
		{
			MenuTree features = new MenuTree("christmasfeatures#features", "FEATURES SELECTION")
			{
				new CheckBox(MenuDisplayMode.Both, "setting:override_loading_screens", "MODIFY LOADING SCREEN BACKGROUNDS")
				.WithGetter(() => Config.OverrideLoadingScreens)
				.WithSetter((x) => Config.OverrideLoadingScreens = x)
				.WithDescription($"{InternalResources.Constants.CHEAT_VISUAL}: Display Christmas themed loading screens.\n(does not apply for [u]Lost to Echoes[/u] loading screens)"),

				new CheckBox(MenuDisplayMode.Both, "setting:cosmetic_screen_snow", "DECORATIVE SNOW")
				.WithGetter(() => Config.ChristmasSnowVisualCheat)
				.WithSetter((x) => Config.ChristmasSnowVisualCheat = x)
				.WithDescription($"{InternalResources.Constants.CHEAT_VISUAL}: Display snow on the car screen.\n(only applies for vehicles having a screen on them)"),

				new CheckBox(MenuDisplayMode.Both, "setting:cosmetic_reindeer_hat", "DECORATIVE REINDEER HAT")
				.WithGetter(() => Config.ReindeerCosmeticVisualCheat)
				.WithSetter((x) => Config.ReindeerCosmeticVisualCheat = x)
				.WithDescription($"{InternalResources.Constants.CHEAT_VISUAL}: Add a cosmetic reindeer hat to the car.\n(may not apply on all cars)")
			};

			MenuTree menu = new MenuTree("christmasfeatures#main.menu", "CHRISTMAS FEATURES")
			{
				new ListBox<ActivationMode>(MenuDisplayMode.Both, "setting:activation_mode", "ACTIVATION MODE")
				.WithGetter(() => Config.ActivationMode)
				.WithSetter((x) => Config.ActivationMode = x)
				.WithEntries(new Dictionary<string, ActivationMode>()
				{
					{ "Always", ActivationMode.Always },
					{ "During December", ActivationMode.DuringDecember },
					{ "Always Except December", ActivationMode.AlwaysExceptDecember },
					{ "First 25 Days Of December", ActivationMode.First25DaysOfDecember },
					{ "Only On Christmas", ActivationMode.OnlyOnDecember24And25 },
					{ "During Christmas Week", ActivationMode.DuringWeekOfChristmas },
					{ "Never", ActivationMode.Never }
				})
				.WithDescription("Select when the mod should be enabled."),

				new ListBox<TimeFormat>(MenuDisplayMode.Both, "setting:time_format", "TIME ZONE FORMAT")
				.WithGetter(() => Config.TimeFormat)
				.WithSetter((x) => Config.TimeFormat = x)
				.WithEntries(MapEnumToListBox<TimeFormat>())
				.WithDescription("Select between local or UTC time zone.\nThis setting is used in date calculations for some activation modes."),

				new SubMenu(MenuDisplayMode.Both, "menu:features", "SELECT FEATURES")
				.NavigatesTo(features)
				.WithDescription("Select which features are active when the mod is enabled.")
			};

			Menus.AddNew(MenuDisplayMode.Both, menu, "Settings for the Christmas Seasonal Features mod");
		}

		private Dictionary<string, T> MapEnumToListBox<T>() where T : Enum
		{
			var ret = new Dictionary<string, T>();

			var keys = Enum.GetNames(typeof(T));
			var values = (T[])Enum.GetValues(typeof(T));

			for (var i = 0; i < keys.Length; i++)
			{
				ret.Add(SplitCamelCase(keys[i]), values[i]);
			}

			return ret;
		}

		public string SplitCamelCase(string str)
		{
			// https://stackoverflow.com/a/5796793
			return Regex.Replace(
				Regex.Replace(
					str,
					@"(\P{Ll})(\P{Ll}\p{Ll})",
					"$1 $2"
				),
				@"(\p{Ll})(\P{Ll})",
				"$1 $2"
			);
		}
	}
}
