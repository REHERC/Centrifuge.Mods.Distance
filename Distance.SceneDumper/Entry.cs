using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Input;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Distance.SceneDumper
{
    [ModEntryPoint("com.github.ciastex/Distance.SceneDumper")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance;

        public IManager Manager { get; set; }

        public Log Logger { get; set; }

        public FileSystem FileSystem { get; set; }

        public ConfigurationLogic Config { get; set; }

        public Dumper Dumper { get; set; }

        public void Initialize(IManager manager)
        {
            Instance = this;
            Manager = manager;

            Logger = LogManager.GetForCurrentAssembly();
            FileSystem = new FileSystem();

            Dumper = new Dumper(FileSystem);

            Config = gameObject.AddComponent<ConfigurationLogic>();
            Config.OnChanged += OnConfigChanged;

            CreateSettingsMenu();

            OnConfigChanged(Config);

            RuntimePatcher.AutoPatch();
        }

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.scenedumper", "Scene Dumper Settings")
            {
                new InputPrompt(MenuDisplayMode.Both, "setting:keybind_dumper_basic", "BASIC DUMP KEY BIND")
                .WithDefaultValue(() => Config.DumpSceneBasic)
                .WithSubmitAction((x) => Config.DumpSceneBasic = x)
                .WithTitle("ENTER KEY BINDING")
                .WithDescription("Set the keyboard shortcut used to make a basic dump."),

                new InputPrompt(MenuDisplayMode.Both, "setting:keybind_dumper_detailed", "DETAILED DUMP KEY BIND")
                .WithDefaultValue(() => Config.DumpSceneDetailed)
                .WithSubmitAction((x) => Config.DumpSceneDetailed = x)
                .WithTitle("ENTER KEY BINDING")
                .WithDescription("Set the keyboard shortcut used to make a detailed dump."),

                new ActionButton(MenuDisplayMode.Both, "setting:open_dumps_folder", "OPEN DUMPS FOLDER")
                .WhenClicked(() =>
                {
                    DirectoryInfo data = new DirectoryInfo(FileSystem.VirtualFileSystemRoot);

                    if (!data.Exists)
                    {
                        data.Create();
                    }

                    Process.Start(new ProcessStartInfo(data.FullName));
                })
                .WithDescription("Opedns the folder containing dump logs.")
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "SCENE DUMPER SETTINGS", "Settings for the Scene Dumper mod.");
        }


        private Hotkey _keybindDumperBasic = null;
        private Hotkey _keybindDumperDetailed = null;

        public void OnConfigChanged(ConfigurationLogic config)
        {
            BindAction(ref _keybindDumperBasic, config.DumpSceneBasic, () => { 
                Logger.Info("Performing basic dump...");
                Dumper.DumpCurrentScene(false);
            });

            BindAction(ref _keybindDumperDetailed, config.DumpSceneDetailed, () => { 
                Logger.Info("Performing detailed dump...");
                Dumper.DumpCurrentScene(true);
            });
        }

        public void BindAction(ref Hotkey unbind, string rebind, Action callback)
        {
            if (unbind != null)
            {
                Manager.Hotkeys.UnbindHotkey(unbind);
            }

            unbind = Manager.Hotkeys.BindHotkey(rebind, callback, true);
        }
    }
}