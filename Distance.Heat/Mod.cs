using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using HarmonyLib;
using Reactor.API.Attributes;
using Reactor.API.Configuration;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using UnityEngine;

namespace Distance.Heat
{
    [ModEntryPoint(ModID)]
    public class Mod : MonoBehaviour
    {
        public const string ModID = "com.github.Seeker14491.Heat";

        private ActivationMode ActivationMode
            => (ActivationMode)_settings.GetItem<int>(Resources.ActivationModeSettingsKey);

        private DisplayMode DisplayMode
            => (DisplayMode)_settings.GetItem<int>(Resources.DisplayModeSettingsKey);

        private DisplayUnits DisplayUnits
            => (DisplayUnits)_settings.GetItem<int>(Resources.DisplayUnitsSettingsKey);

        private Settings _settings = new Settings("heat");
        private Log _log = LogManager.GetForCurrentAssembly();
        private readonly GameState _gameState = new GameState();

        private bool _toggled;
        private UILabel _watermark;

        public void Initialize(IManager manager)
        {
            InitializeSettings();

            manager.Hotkeys.Bind(_settings.GetItem<string>(Resources.ToggleHotkeySettingsKey), () =>
            {
                _toggled = !_toggled;

                if (_watermark != null)
                    _watermark.text = "";
            });

            Events.MainMenu.Initialized.Subscribe((data) =>
            {
                if (DisplayMode == DisplayMode.Watermark)
                    _watermark = GetAndActivateWatermark();

                CreateMenuVisualTree();
            });
        }

        public void Update()
        {
            _gameState?.Update();

            var text = "";
            if (DisplayEnabled())
            {
                text = DisplayText(DisplayUnits);
            }

            switch (DisplayMode)
            {
                case DisplayMode.Hud:
                    if (DisplayEnabled())
                    {
                        SetHudText(text);
                    }

                    break;
                case DisplayMode.Watermark:
                    if (_watermark != null)
                        _watermark.text = text;
                    break;
            }
        }

        private void CreateMenuVisualTree()
        {
            var settingsMenu = new MenuTree(ModID, "Heat Display Settings")
            {
                new InputPrompt(MenuDisplayMode.Both, "setting:toggle_hotkey", "SET TOGGLE HOTKEY")
                    .WithTitle("SET TOGGLE HOTKEY")
                    .WithSubmitAction(str =>
                    {
                        _settings[Resources.ToggleHotkeySettingsKey] = str;
                        _settings.SaveIfDirty();
                    }),

                new ListBox<DisplayUnits>(MenuDisplayMode.Both, "setting:display_units", "DISPLAY UNITS")
                    .WithEntries(MapEnumToListBox<DisplayUnits>())
                    .WithGetter(() => DisplayUnits)
                    .WithSetter((sel) =>
                    {
                        _settings[Resources.DisplayUnitsSettingsKey] = sel;
                        _settings.SaveIfDirty();
                    }),

                new ListBox<DisplayMode>(MenuDisplayMode.Both, "setting:display_mode", "DISPLAY MODE")
                    .WithEntries(MapEnumToListBox<DisplayMode>())
                    .WithGetter(() => DisplayMode)
                    .WithSetter((sel) =>
                    {
                        _settings[Resources.DisplayModeSettingsKey] = sel;
                        _settings.SaveIfDirty();
                    }),

                new ListBox<ActivationMode>(MenuDisplayMode.Both, "setting:activation_mode", "ACTIVATION MODE")
                    .WithEntries(MapEnumToListBox<ActivationMode>())
                    .WithGetter(() => ActivationMode)
                    .WithSetter((sel) =>
                    {
                        _settings[Resources.ActivationModeSettingsKey] = sel;
                        _settings.SaveIfDirty();
                    }),

                new FloatSlider(MenuDisplayMode.Both, "setting:warning_threshold", "WARNING THRESHOLD")
                    .LimitedByRange(0.0f, 1.0f)
                    .WithGetter(() => _settings.GetItem<float>(Resources.WarningThresholdSettingsKey))
                    .WithSetter((val) =>
                    {
                        _settings[Resources.WarningThresholdSettingsKey] = val;
                        _settings.SaveIfDirty();
                    })
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "HEAT DISPLAY", "Configure the Heat mod.");
        }

        private static UILabel GetAndActivateWatermark()
        {
            var anchorAlphaVersion = GameObject.Find("UI Root").transform.Find("Panel/Anchor : AlphaVersion");
            var alphaVersion = anchorAlphaVersion.Find("AlphaVersion");

            anchorAlphaVersion.gameObject.SetActive(true);
            alphaVersion.gameObject.SetActive(true);

            return alphaVersion.GetComponent<UILabel>();
        }

        private string Speed(DisplayUnits displayUnit)
        {
            var speed = _gameState.CarStats
                ? Traverse.Create(_gameState.CarStats).Method("GetSpeed").GetValue<float>()
                : 0f;
            switch (displayUnit)
            {
                case DisplayUnits.Automatic:
                    if (_gameState.GeneralSettings && _gameState.GeneralSettings.Units_ == global::Units.Imperial)
                    {
                        goto case DisplayUnits.Mph;
                    }
                    else
                    {
                        goto case DisplayUnits.Kph;
                    }
                case DisplayUnits.Kph:
                    var kph = speed * 3.6f;
                    return $"{kph:N0} KPH";
                case DisplayUnits.Mph:
                    var mph = speed * 2.23694f;
                    return $"{mph:N0} MPH";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool DisplayEnabled()
        {
            if (!_gameState.CarLogic)
                return false;


            var warningThreshold = _settings.GetItem<float>(Resources.WarningThresholdSettingsKey);

            return ActivationMode == ActivationMode.Always
                   || ActivationMode == ActivationMode.Warning && GetHeatLevel() > warningThreshold
                   || ActivationMode == ActivationMode.Toggle && _toggled;
        }

        private void InitializeSettings()
        {
            _settings.GetOrCreate<string>(Resources.ToggleHotkeySettingsKey, "LeftControl+H");
            _settings.GetOrCreate<DisplayUnits>(Resources.DisplayUnitsSettingsKey, DisplayUnits.Automatic);
            _settings.GetOrCreate<DisplayMode>(Resources.DisplayModeSettingsKey, DisplayMode.Watermark);
            _settings.GetOrCreate<ActivationMode>(Resources.ActivationModeSettingsKey, ActivationMode.Always);
            _settings.GetOrCreate<float>(Resources.WarningThresholdSettingsKey, 0.8f);

            _settings.SaveIfDirty();
        }

        private float GetHeatLevel()
            => _gameState.CarLogic ? _gameState.CarLogic.Heat_ : 0f;

        private string DisplayText(DisplayUnits displayUnit)
            => $"{GetHeatLevel():P0} Heat\n{Speed(displayUnit)}";

        private void SetHudText(string text)
        {
            if (_gameState.HoverScreenEmitter)
                _gameState.HoverScreenEmitter.SetTrickText(new TrickyTextLogic.TrickText(3.0f, -1,
                    TrickyTextLogic.TrickText.TextType.standard, text));
        }

        private Dictionary<string, T> MapEnumToListBox<T>() where T : Enum
        {
            var ret = new Dictionary<string, T>();

            var keys = Enum.GetNames(typeof(T));
            var values = (T[])Enum.GetValues(typeof(T));

            for (var i = 0; i < keys.Length; i++)
                ret.Add(keys[i], values[i]);

            return ret;
        }
    }
}