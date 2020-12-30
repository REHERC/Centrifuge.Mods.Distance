using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Distance.Heat.Enums;
using Reactor.API.Attributes;
using Reactor.API.Input;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);
			Instance = this;
			Manager = manager;

			Flags.SubscribeEvents();

			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigurationLogic>();

			Config.OnChanged += OnConfigChanged;

			OnConfigChanged(Config);

			CreateSettingsMenu();

			RuntimePatcher.AutoPatch();
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

		#region Utilities
		private Dictionary<string, T> MapEnumToListBox<T>() where T : Enum
		{
			var result = new Dictionary<string, T>();

			var keys = Enum.GetNames(typeof(T));
			var values = (T[])Enum.GetValues(typeof(T));

			for (int index = 0; index < keys.Length; index++)
			{
				result.Add(SplitCamelCase(keys[index]), values[index]);
			}

			return result;
		}

		private string SplitCamelCase(string str)
		{
			// https://stackoverflow.com/a/5796793
			return Regex.Replace(
				Regex.Replace(
					str,
					@"(\P{Ll})(\P{Ll}\p{Ll})",
					"$1 $2"
				),
				@"(\p{Ll})(\P{Ll})",
				"$1 $2"
			);
		}
		#endregion

		#region Data
		public bool DisplayCondition => Config.ActivationMode == ActivationMode.Always ||
			(Config.ActivationMode == ActivationMode.Warning && Vehicle.HeatLevel >= Config.WarningTreshold) ||
			(Config.ActivationMode == ActivationMode.Toggle && Toggled);

		public bool Toggled { get; set; }

		public string Text => $"{GetHeatLevel()}\n{GetSpeed()}";

		public string GetHeatLevel()
		{
			string percent = Mathf.RoundToInt(100 * Mathf.Clamp(Vehicle.HeatLevel, 0, 1)).ToString();
            while (percent.Length< 3)
			{
                percent = $"0{percent}";
			}

			return $"{percent} %";
		}

		private string GetSpeed()
		{
			if (G.Sys.GameManager_.IsModeStarted_)
			{
				switch (Centrifuge.Distance.Game.Options.General.Units)
				{
					case Units.Metric:
						return $"{Mathf.RoundToInt(Vehicle.VelocityKPH)} KM/H";
					case Units.Imperial:
						return $"{Mathf.RoundToInt(Vehicle.VelocityMPH)} MPH";
					default:
						return string.Empty;
				}
			}
			else
			{
				return string.Empty;
			}
		}
		#endregion

		#region Settings Changed
		public void OnConfigChanged(ConfigurationLogic config)
		{
			BindAction(ref _keybindToggle, config.ToggleHotkey, () =>
			{
				Toggled = !Toggled;

				if (!Toggled)
				{
					foreach (TrickyTextLogic trickText in FindObjectsOfType<TrickyTextLogic>())
					{
						trickText.Clear();
					}
				}
			});
		}
		#endregion

		#region Key Bindings

		private Hotkey _keybindToggle = null;

		public void BindAction(ref Hotkey unbind, string rebind, Action callback)
		{
			if (unbind != null)
			{
				Manager.Hotkeys.UnbindHotkey(unbind);
			}

			unbind = Manager.Hotkeys.BindHotkey(rebind, callback, true);
		}
		#endregion
	}
}