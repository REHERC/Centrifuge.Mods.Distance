using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.MenuUtilities
{
	[ModEntryPoint("com.github.reherc/Distance.MenuUtilities")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public ConfigurationLogic Config { get; private set; }

		public MonoBehaviour[] Scripts { get; set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;
			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigurationLogic>();

			RuntimePatcher.AutoPatch();

			CreateSettingsMenu();
		}

		public void CreateSettingsMenu()
		{
			// TODO: Update readme instructions
			MenuTree settingsMenu = new MenuTree("menu.mod.menuutilities", "Menu Settings")
			{
				new CheckBox(MenuDisplayMode.Both, "setting:enable_delete_playlist", "SHOW LEVEL PLAYLIST REMOVE BUTTON")
					.WithGetter(() => Config.EnableDeletePlaylistButton)
					.WithSetter((x) => Config.EnableDeletePlaylistButton = x)
					.WithDescription("Display a button to delete playlists in the level grid menu."),

				new CheckBox(MenuDisplayMode.Both, "setting:enable_car_hex_input", "SHOW HEXADECIMAL CAR COLOR INPUT")
					.WithGetter(() => Config.EnableHexColorInput)
					.WithSetter((x) => Config.EnableHexColorInput = x)
					.WithDescription("Display a button to modify the hexadecimal color value as text in the car customization menu."),
			};

			Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "MENU UTILITIES", "Settings for the Menu Utilities mod.");
		}
	}
}