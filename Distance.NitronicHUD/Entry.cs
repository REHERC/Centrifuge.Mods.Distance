﻿#pragma warning disable IDE0028
using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Distance.NitronicHUD.Scripts;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System.Linq;
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

        public void Load()
        {
            Scripts = new MonoBehaviour[2]
            {
                gameObject.AddComponent<VisualCountdown>(),
                gameObject.AddComponent<VisualDisplay>()
            };
        }

        public void CreateSettingsMenu()
        {
            MenuTree displayMenu = new MenuTree("menu.mod.nitronichud#interface", "Interface Options")
            {
                /*new CheckBox(MenuDisplayMode.Both, "setting:display_countdown", "SHOW COUNTDOWN")
                .WithGetter(() => Config.DisplayCountdown)
                .WithSetter(x => Config.DisplayCountdown = x)
                .WithDescription("Displays the 3... 2... 1... RUSH countdown when playing a level."),*/

                new CheckBox(MenuDisplayMode.Both, "setting:display_overheat", "SHOW OVERHEAT METERS")
                .WithGetter(() => Config.DisplayHeatMeters)
                .WithSetter(x => Config.DisplayHeatMeters = x)
                .WithDescription("Displays overheat indicator bars in the lower screen corners."),

                new CheckBox(MenuDisplayMode.Both, "setting:display_timer", "SHOW TIMER")
                .WithGetter(() => Config.DisplayTimer)
                .WithSetter(x => Config.DisplayTimer = x)
                .WithDescription("Displays the timer at the bottom of the screen."),

                new IntegerSlider(MenuDisplayMode.Both, "setting:overheat_scale", "OVERHEAT SCALE")
                .WithDefaultValue(20)
                .WithGetter(() => Mathf.RoundToInt(Config.HeatMetersScale * 20.0f))
                .WithSetter(x => Config.HeatMetersScale = x / 20.0f)
                .LimitedByRange(1, 50)
                .WithDescription("Set the size of the overheat bars."),

                new IntegerSlider(MenuDisplayMode.Both, "setting:overheat_horizontal_offset", "OVERHEAT HORIZONTAL POSITION")
                .WithDefaultValue(0)
                .WithGetter(() => Config.HeatMetersHorizontalOffset)
                .WithSetter(x => Config.HeatMetersHorizontalOffset = x)
                .LimitedByRange(-200, 200)
                .WithDescription("Set the horizontal position offset of the overheat bars."),

                new IntegerSlider(MenuDisplayMode.Both, "setting:overheat_vertical_offset", "OVERHEAT VERTICAL POSITION")
                .WithDefaultValue(0)
                .WithGetter(() => Config.HeatMetersVerticalOffset)
                .WithSetter(x => Config.HeatMetersVerticalOffset = x)
                .LimitedByRange(-100, 100)
                .WithDescription("Set the vertical position offset of the overheat bars."),

                new IntegerSlider(MenuDisplayMode.Both, "setting:timer_scale", "TIMER SCALE")
                .WithDefaultValue(20)
                .WithGetter(() => Mathf.RoundToInt(Config.TimerScale * 20.0f))
                .WithSetter(x => Config.TimerScale = x / 20.0f)
                .LimitedByRange(1, 50)
                .WithDescription("Set the size of the timer."),

                new IntegerSlider(MenuDisplayMode.Both, "setting:timer_vertical_offset", "TIMER VERTICAL OFFSET")
                .WithDefaultValue(0)
                .WithGetter(() => Config.TimerVerticalOffset)
                .WithSetter(x => Config.TimerVerticalOffset = x)
                .LimitedByRange(-100, 100)
                .WithDescription("Set the vertical position of the timer.")
            };

            MenuTree audioMenu = new MenuTree("menu.mod.nitronichud#audio", "Audio Options");

            MenuTree settingsMenu = new MenuTree("menu.mod.nitronichud", "Nitronic HUD Settings");

            settingsMenu.Add(
                new SubMenu(MenuDisplayMode.Both, "menu:interface", "DISPLAY")
                .NavigatesTo(displayMenu)
                .WithDescription("Configure settings for the visual interface."));

            if (Application.platform != RuntimePlatform.LinuxPlayer)
            {
                settingsMenu.Add(
                new SubMenu(MenuDisplayMode.Both, "menu:interface", "AUDIO".Colorize(Colors.gray))
                .NavigatesTo(audioMenu)
                .WithDescription("Configure audio settings for the countdown announcer."));
            }

            settingsMenu.Add(
                new ActionButton(MenuDisplayMode.MainMenu, "menu:interface", "PREVIEW SETTINGS".Colorize(Colors.gray))
                .WhenClicked(() => {
                    MessageBox.Create("This feature isn't implemented yet but will be in a future release.", "ERROR")
                    .SetButtons(MessageButtons.Ok)
                    .Show();
                })
                .WithDescription("Start the animation sequence thet plays when a level starts to preview the settings."));

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "NITRONIC HUD [1E90FF](BETA)[-]", "Settings for the Nitronic HUD mod.");
        }
    }
}