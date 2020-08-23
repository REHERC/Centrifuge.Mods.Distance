using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Distance.Heat.Enums;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.Heat
{
    [ModEntryPoint("com.github.Seeker14491/Heat")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance;

        public IManager Manager { get; set; }

        public Log Logger { get; set; }

        public ConfigurationLogic Config { get; private set; }

        private GameStateLogic GameState_ { get; set; }

        public void Initialize(IManager manager)
        {
            Instance = this;
            Manager = manager;
            Logger = LogManager.GetForCurrentAssembly();
            Config = gameObject.AddComponent<ConfigurationLogic>();

            GameState_ = gameObject.AddComponent<GameStateLogic>();

            RuntimePatcher.AutoPatch();

            CreateSettingsMenu();
        }

        private void CreateSettingsMenu()
        {
            var settingsMenu = new MenuTree("menu.mod.heat", "Heat Display Settings")
            {
                new InputPrompt(MenuDisplayMode.Both, "setting:toggle_hotkey", "SET TOGGLE HOTKEY")
                .WithTitle("SET TOGGLE HOTKEY")
                .WithDefaultValue(() => Config.ToggleHotkey)
                .WithSubmitAction(x => Config.ToggleHotkey = x),

                new ListBox<DisplayMode>(MenuDisplayMode.Both, "setting:display_mode", "DISPLAY MODE")
                .WithEntries(MapEnumToListBox<DisplayMode>())
                .WithGetter(() => Config.DisplayMode)
                .WithSetter((x) => Config.DisplayMode = x),

                new ListBox<ActivationMode>(MenuDisplayMode.Both, "setting:activation_mode", "ACTIVATION MODE")
                .WithEntries(MapEnumToListBox<ActivationMode>())
                .WithGetter(() => Config.ActivationMode)
                .WithSetter((x) => Config.ActivationMode = x),

                new FloatSlider(MenuDisplayMode.Both, "setting:warning_threshold", "WARNING THRESHOLD")
                .LimitedByRange(0.0f, 1.0f)
                .WithGetter(() => Config.WarningTreshold)
                .WithSetter((x) => Config.WarningTreshold = x)
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "HEAT DISPLAY", "Configure the Heat mod.");
        }

        private Dictionary<string, T> MapEnumToListBox<T>() where T : Enum
        {
            var ret = new Dictionary<string, T>();

            var keys = Enum.GetNames(typeof(T));
            var values = (T[])Enum.GetValues(typeof(T));

            for (var i = 0; i < keys.Length; i++)
            {
                ret.Add(keys[i], values[i]);
            }

            return ret;
        }
    }
}