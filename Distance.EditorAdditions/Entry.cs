using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using System.Linq;
using UnityEngine;

namespace Distance.EditorAdditions
{
	[ModEntryPoint("com.github.reherc/Distance.EditorAdditions")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public TrackNodeColors TrackNodeColors { get; set; }

		public ConfigurationLogic Config { get; private set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;
			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigurationLogic>();

			TrackNodeColors = TrackNodeColors.FromSettings("SplineColors.json");
			TrackNodeColors.OnFileReloaded += ReloadTrackNodeColors;

			RuntimePatcher.AutoPatch();

			CreateSettingsMenu();

			foreach (var mesh in Resources.FindObjectsOfTypeAll<Mesh>())
			{
				Logger.Error($"Mesh: {mesh.name}");
			}
		}

		private void ReloadTrackNodeColors(object sender, EventArgs e)
		{
			foreach (var node in FindObjectsOfType<TrackManipulatorNode>())
			{
				node.SetColorAndMesh();
			}
		}

		public void CreateSettingsMenu()
		{
			MenuTree settingsMenu;

			if (!DevMode)
			{
				settingsMenu = new MenuTree("menu.mod.editoradditions", "Editor Additions Settings")
				{
					new CheckBox(MenuDisplayMode.MainMenu, "setting:show_dev_folder", "ENABLE DEV OBJECTS FOLDER")
					.WithGetter(() => Config.DevFolderEnabled)
					.WithSetter((x) => Config.DevFolderEnabled = x)
					.WithDescription("Enables the Dev folder in the level editor Library tab.\nSome of the objects in this folder might not work properly, be careful when using them!"),

					new CheckBox(MenuDisplayMode.MainMenu, "setting:advanced_music_selection", "ADVANCED MUSIC SELECTION")
					.WithGetter(() => Config.AdvancedMusicSelection)
					.WithSetter((x) => Config.AdvancedMusicSelection = x)
					.WithDescription("Display hidden dev music cues in the level editor music selector window."),

					new CheckBox(MenuDisplayMode.MainMenu, "setting:display_workshop_levels", "LIST WORKSHOP LEVELS")
					.WithGetter(() => Config.DisplayWorkshopLevels)
					.WithSetter((x) => Config.DisplayWorkshopLevels = x)
					.WithDescription("Allows to open the workshop levels from the level editor (read only)."),

					new ActionButton(MenuDisplayMode.Both, "info:devmode", "DEV MODE".Colorize(Colors.tan))
					.WhenClicked(() => { })
					.WithDescription("Start the game with the \"-dev\" console argument to enable dev mode.\nThis will override the previous settings to be enabled, however there is no guarantee it won't have other side effects.")
				};
			}
			else
			{
				settingsMenu = new MenuTree("menu.mod.editoradditions", "Editor Additions Settings")
				{
					new ActionButton(MenuDisplayMode.Both, "info:devmode-enabled", "DEV MODE ENABLED".Colorize(Colors.cadetBlue))
					.WhenClicked(() => { })
					.WithDescription("To disable, remove the \"-dev\" command line argument from the game launch options.")
				};
			}

			Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "EDITOR ADDITIONS", "Settings for the EditorAdditions mod.");
		}

		public static bool DevMode => IsCommandLineSwitchPresent("-dev");

		public static bool IsCommandLineSwitchPresent(string item)
		{
			return Environment.GetCommandLineArgs().Select(arg => arg.ToLower()).Contains(item);
		}
	}
}