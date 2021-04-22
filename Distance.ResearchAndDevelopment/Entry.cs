﻿using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Events.MainMenu;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System.Collections;
using UnityEngine;
using System;
using Centrifuge.Distance.Data;
using System.Runtime.InteropServices;

namespace Distance.ResearchAndDevelopment
{
	[ModEntryPoint("com.github.reherc/Distance.ResearchAndDevelopment")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;
			Logger = LogManager.GetForCurrentAssembly();

			RuntimePatcher.AutoPatch();

			//Initialized.Subscribe(OnMainMenuInitialized);
			CreateSettingsMenu();
		}

		public void CreateSettingsMenu()
		{
			Menus.AddNew(MenuDisplayMode.Both, new MenuTree("menu.r&d.main", "Research and Development Test Mod")
			{
				new ActionButton(MenuDisplayMode.Both, "gc_collect", "System.GC.Collect()")
				.WhenClicked(() => GC.Collect())
				.WithDescription("Forces immediate garbage collection by the .Net runtime"),

				new ActionButton(MenuDisplayMode.Both, "resources_unload", "UnityEngine.Resources.UnloadUnusedAssets()")
				.WhenClicked(() => Resources.UnloadUnusedAssets())
				.WithDescription("Forces the Unity engine to unload any asset that is loaded and unused at the moment"),

				new ActionButton(MenuDisplayMode.Both, "print_alloc", "System.GC.GetTotalMemory(false)")
				.WhenClicked(() => MessageBox.Create(GC.GetTotalMemory(false).ToString(), "").SetButtons(MessageButtons.Ok).Show())
				.WithDescription("Print the number of bytes of managed memory allocated to the .Net runtime (for this process only)")
			});
			/*
			MenuTree advancedDisplayMenu = new MenuTree("menu.mod.nitronichud#interface.advanced", "Advanced Interface Options")
			{
				new FloatSlider(MenuDisplayMode.Both, "setting:heat_blink_start_amount", "HEAT BLINK START AMOUNT")
				.LimitedByRange(0.0f, 1.0f)
				.WithDefaultValue(0.7f)
				.WithGetter(() => Config.HeatBlinkStartAmount)
				.WithSetter(x => Config.HeatBlinkStartAmount = x)
				.WithDescription("Set the heat treshold after which the hud starts to blink."),

				new FloatSlider(MenuDisplayMode.Both, "setting:heat_blink_frequence", "HEAT BLINK FREQUENCE")
				.LimitedByRange(0.0f, 10.0f)
				.WithDefaultValue(2.0f)
				.WithGetter(() => Config.HeatBlinkFrequence)
				.WithSetter(x => Config.HeatBlinkFrequence = x)
				.WithDescription("Set the hud blink rate (per second)."),

				new FloatSlider(MenuDisplayMode.Both, "setting:heat_blink_frequence_boost", "HEAT BLINK FREQUENCE BOOST")
				.LimitedByRange(0.0f, 10.0f)
				.WithDefaultValue(1.15f)
				.WithGetter(() => Config.HeatBlinkFrequenceBoost)
				.WithSetter(x => Config.HeatBlinkFrequenceBoost = x)
				.WithDescription("Sets the blink rate boost.\nThe blink rate at 100% heat is the blink rate times this value (set this to 1 to keep the rate constant)."),

				new FloatSlider(MenuDisplayMode.Both, "setting:heat_blink_amount", "HEAT BLINK AMOUNT")
				.LimitedByRange(0.0f, 1.0f)
				.WithDefaultValue(0.7f)
				.WithGetter(() => Config.HeatBlinkAmount)
				.WithSetter(x => Config.HeatBlinkAmount = x)
				.WithDescription("Sets the color intensity of the overheat blink animation (lower values means smaller color changes)."),

				new FloatSlider(MenuDisplayMode.Both, "setting:heat_flame_amount", "HEAT FLAME AMOUNT")
				.WithDefaultValue(0.5f)
				.LimitedByRange(0.0f, 1.0f)
				.WithGetter(() => Config.HeatFlameAmount)
				.WithSetter(x => Config.HeatFlameAmount = x)
				.WithDescription("Sets the color intensity of the overheat flame animation (lower values means smaller color changes).")
			};

			MenuTree displayMenu = new MenuTree("menu.mod.nitronichud#interface", "Interface Options")
			{
				new CheckBox(MenuDisplayMode.Both, "setting:display_countdown", "SHOW COUNTDOWN")
				.WithGetter(() => Config.DisplayCountdown)
				.WithSetter(x => Config.DisplayCountdown = x)
				.WithDescription("Displays the 3... 2... 1... RUSH countdown when playing a level."),

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
				.WithDescription("Set the vertical position of the timer."),

				new SubMenu(MenuDisplayMode.Both, "menu:interface.advanced", "ADVANCED SETTINGS")
				.NavigatesTo(advancedDisplayMenu)
				.WithDescription("Configure advanced settings for the hud."),

				new ActionButton(MenuDisplayMode.Both, "action:preview_hud", "PREVIEW HUD")
				.WhenClicked(() => VisualDisplay.ForceDisplay = !VisualDisplay.ForceDisplay)
				.WithDescription("Show the hud to preview changes.")
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
				.WhenClicked(() =>
				{
					MessageBox.Create("This feature isn't implemented yet but will be in a future release.", "ERROR")
					.SetButtons(MessageButtons.Ok)
					.Show();
				})
				.WithDescription("Start the animation sequence thet plays when a level starts to preview the settings."));

			//Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "NITRONIC HUD [1E90FF](BETA)[-]", "Settings for the Nitronic HUD mod.");
			Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "NITRONIC HUD [FFBF1E](RELEASE CANDIDATE)[-]", "Settings for the Nitronic HUD mod.");
			*/
		}

		private void OnMainMenuInitialized(Initialized.Data data)
		{
			/*SteamworksUGC ugc = G.Sys.SteamworksManager_.UGC_;

			FileSystem files = new FileSystem();

			string filePath = Path.Combine(files.VirtualFileSystemRoot, "obscuration");

			SteamworksUGC.LevelStuff wItem = new SteamworksUGC.LevelStuff
			(
				"Obscuration [preview]",
				"obscuration",
				"MyLevels/obscuration-preview.bytes",
				"This is a level I made for a campaign project that I will release in the future, meanwhile here's a very short preview :D",
				new List<string>()
				{
					"Level"
				}
			);

			MessageBox.Create(filePath, "UPLOAD")
			.SetButtons(Centrifuge.Distance.Data.MessageButtons.YesNo)
			.OnConfirm(() =>
			{
				ugc.SteamFileWrite("obscuration", filePath);
				ugc.SteamFileWrite("obscuration.png", filePath + ".png");

				ugc.AddTask(new SteamworksUGC.PublishWorkshopFileTask(ugc, wItem, SteamworksManager.AppID_, (success, steam_ugc) => { }));

			})
			.Show();*/

			//Task.Run(MyTask);
		}

		private IEnumerator MyTask(Task.Status status)
		{
			const int max = 500;

			status.SetText("Setting up...");
			status.SetProgress(0, 1);

			yield return Task.Wait(1.5f);

			for (uint i = 1; i <= max; ++i)
			{
				status.SetText($"Running task {i} of {max}...");
				status.SetProgress(i, max);

				yield return Task.Wait(0.05f);
			}

			status.SetText("Finishing...");
			status.SetProgress(1, 1);

			yield return Task.Wait(2.0f);
		}
	}
}