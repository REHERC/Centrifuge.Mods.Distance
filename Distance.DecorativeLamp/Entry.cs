using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Distance.DecorativeLamp.Enums;
using Events.DecorativeLamp;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Distance.DecorativeLamp
{
	[ModEntryPoint("com.github.reherc/Distance.DecorativeLamp")]
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
			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigurationLogic>();

			RuntimePatcher.AutoPatch();

			CreateSettingsMenu();
		}

		public void CreateSettingsMenu()
		{
			// TODO: Update readme instructions
			MenuTree settingsMenu = new MenuTree("menu.mod.decorativelamp", "Decorative Lamp Settings")
			{
				new CheckBox(MenuDisplayMode.Both, "setting:enable_lamp", "LAMP ENABLED")
					.WithGetter(() => Config.Enabled)
					.WithSetter((x) => Config.Enabled = x)
					.WithDescription(string.Format("{0}: Enlighten your way through the Array.", "Visual".Colorize(Colors.yellowGreen))),

				new ListBox<LampModel>(MenuDisplayMode.Both, "setting:lamp_model", "3D MODEL")
					.WithEntries(MapEnumToListBox<LampModel>())
					.WithGetter(() => Config.MeshModel)
					.WithSetter(x => {
						Config.MeshModel = x;
						ChangeLampModel.Broadcast(new ChangeLampModel.Data(x));
					})
					.WithDescription(string.Format("{0}: Select the lamp 3D mesh.", "Visual".Colorize(Colors.yellowGreen))),

				new CheckBox(MenuDisplayMode.Both, "setting:enable_spin", "SPINNING LAMP")
					.WithGetter(() => Config.Spin)
					.WithSetter((x) => Config.Spin = x)
					.WithDescription(string.Format("{0}: Make the lamp spin.", "Visual".Colorize(Colors.yellowGreen))),

				new IntegerSlider(MenuDisplayMode.Both, "setting:spin_speed", "LAMP SPIN SPEED")
					.WithGetter(() => Config.SpinSpeed)
					.WithSetter((x) => Config.SpinSpeed = x)
					.LimitedByRange(-1000, 1000)
					.WithDefaultValue(50)
					.WithDescription(string.Format("{0}: Set the lamp spin speed.", "Visual".Colorize(Colors.yellowGreen))),

				new IntegerSlider(MenuDisplayMode.Both, "setting:lamp_scale", "LAMP SCALE")
					.WithGetter(() => Mathf.RoundToInt(Config.LampScale * 100.0f))
					.WithSetter((x) => Config.LampScale = x / 100.0f)
					.LimitedByRange(0, 500)
					.WithDefaultValue(100)
					.WithDescription(string.Format("{0}: Set the lamp model size.", "Visual".Colorize(Colors.yellowGreen))),

				new IntegerSlider(MenuDisplayMode.Both, "setting:light_intensity", "LIGHT INTENSITY")
					.WithGetter(() => Mathf.RoundToInt(Config.LightIntensity * 100.0f))
					.WithSetter((x) => Config.LightIntensity = x / 100.0f)
					.LimitedByRange(0, 1000)
					.WithDefaultValue(75)
					.WithDescription(string.Format("{0}: Set the lamp brightness.", "Visual".Colorize(Colors.yellowGreen))),

				new IntegerSlider(MenuDisplayMode.Both, "setting:light_range", "LIGHT RANGE")
					.WithGetter(() => Config.LightRange)
					.WithSetter((x) => Config.LightRange = x)
					.LimitedByRange(0, 100)
					.WithDefaultValue(20)
					.WithDescription(string.Format("{0}: Set the lamp light distance.", "Visual".Colorize(Colors.yellowGreen))),

				new IntegerSlider(MenuDisplayMode.Both, "setting:flare_brightness", "FLARE BRIGHTNESS")
					.WithGetter(() => Mathf.RoundToInt(Config.LightRange * 10.0f))
					.WithSetter((x) => Config.FlareBrightness = x / 10.0f)
					.LimitedByRange(0, 20)
					.WithDefaultValue(10)
					.WithDescription(string.Format("{0}: Set the lens flare brightness.", "Visual".Colorize(Colors.yellowGreen)))
			};

			Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "DECORATIVE LAMP", "Settings for the Decorative Lamp mod.");
		}

		private Dictionary<string, T> MapEnumToListBox<T>() where T : Enum
		{
			var ret = new Dictionary<string, T>();

			var keys = Enum.GetNames(typeof(T));
			var values = (T[])Enum.GetValues(typeof(T));

			for (var i = 0; i < keys.Length; i++)
			{
				ret.Add(SplitCamelCase(keys[i]), values[i]);
			}

			return ret;
		}

		public string SplitCamelCase(string str)
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
	}
}