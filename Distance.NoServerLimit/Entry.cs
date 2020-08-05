using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using UnityEngine;

namespace Distance.NoServerLimit
{
    [ModEntryPoint("eu.vddcore/NoServerLimit")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance;

        public IManager Manager { get; set; }

        public Log Logger { get; set; }

        public ConfigurationLogic Config { get; private set; }

        public void Initialize(IManager manager)
        {
            Instance = this;
            Manager = manager;

            Logger = LogManager.GetForCurrentAssembly();
            Config = gameObject.AddComponent<ConfigurationLogic>();

            CreateSettingsMenu();

            RuntimePatcher.AutoPatch();

            Logger.Info("No Server Limit: Hello, world!");
        }

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.noserverlimit", "No Server Limit Settings")
            {
                new InputPrompt(MenuDisplayMode.MainMenu, "setting:set_server_limit", "SET MAXIMUM SERVER SLOT COUNT")
                    .WithDefaultValue(() => Config.MaxPlayerCount.ToString())
                    .WithSubmitAction((x) => {
                        if (int.TryParse(x, out int result))
                        {
                            Config.MaxPlayerCount = result;
                        }
                        else
                        {
                            Logger.Warning("Failed to parse user input. Setting defaults.");
                            Config.MaxPlayerCount = 32;
                        }
                     })
                    .WithTitle("ENTER SLOT COUNT")
                    .WithDescription("Set the maximum supported server slot count.")
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "NO SERVER LIMIT SETTINGS", "Change settings of the No Server Limit mod.");
        }
    }
}