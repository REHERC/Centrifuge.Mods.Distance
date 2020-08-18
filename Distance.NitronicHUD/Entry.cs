using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Distance.NitronicHUD.Scripts;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.NitronicHUD
{
    [ModEntryPoint("com.github.reherc/Distance.NitronicHUD")]
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

        public void LateInitialize(IManager _)
        {
            Scripts = new MonoBehaviour[]
            {
                gameObject.AddComponent<VisualCountdown>()
            };
        }

        public void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.nitronichud", "Nitronic HUD Settings")
            {
                
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "NITRONIC HUD", "Settings for the Nitronic HUD mod.");
        }
    }
}