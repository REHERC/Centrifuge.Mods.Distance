using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Configuration;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;

namespace ExampleNamespace
{
    [ModEntryPoint(ModID)]
    public class Mod
    {
        public const string ModID = "eu.vddcore/NoServerLimit";

        internal static Log Log = LogManager.GetForCurrentAssembly();
        internal static Settings Settings { get; private set; }

        public void Initialize(IManager manager)
        {
            Settings = new Settings("server");
            ValidateSettings();
            CreateSettingsMenu();

            RuntimePatcher.AutoPatch();

            Log.Info("No Server Limit: Hello, world!");
        }

        private void ValidateSettings()
        {
            if (!Settings.ContainsKey("MaxPlayerCount"))
            {
                Settings["MaxPlayerCount"] = 32;
            }

            Settings.Save();
        }

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.noserverlimit", "No Server Limit Settings")
            {
                new InputPrompt(MenuDisplayMode.MainMenu, "setting:set_server_limit", "SET MAXIMUM SERVER SLOT COUNT")
                    .WithDefaultValue(() => Settings.GetItem<int>("MaxPlayerCount").ToString())
                    .WithSubmitAction((x) => {
                        try
                        {
                            Settings["MaxPlayerCount"] = int.Parse(x);
                        }
                        catch
                        {
                            Log.Warning("Failed to parse user input. Setting defaults.");
                            Settings["MaxPlayerCount"] = 32;
                        }

                        Settings.Save();
                     })
                    .WithTitle("ENTER SLOT COUNT")
                    .WithDescription("Set the maximum supported server slot count.")
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "NO SERVER LIMIT SETTINGS", "Change settings of the No Server Limit mod.");
        }
    }
}