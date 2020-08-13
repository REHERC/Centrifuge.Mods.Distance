using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
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
            Instance = this;
            Manager = manager;
            Logger = LogManager.GetForCurrentAssembly();
            Config = gameObject.AddComponent<ConfigurationLogic>();

            RuntimePatcher.AutoPatch();

            CreateSettingsMenu();
        }

        public void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.decorativelamp", "Decorative Lamp Settings")
            {
                new CheckBox(MenuDisplayMode.Both, "setting:enable_lamp", "LAMP ENABLED")
                    .WithGetter(() => Config.Enabled)
                    .WithSetter((x) => Config.Enabled = x)
                    .WithDescription(string.Format("{0}: Enlighten your way through the Array.", "Visual".Colorize(Colors.yellowGreen))),

                new CheckBox(MenuDisplayMode.Both, "setting:enable_spin", "SPINNING LAMP")
                    .WithGetter(() => Config.Spin)
                    .WithSetter((x) => Config.Spin = x)
                    .WithDescription(string.Format("{0}: Make the lamp spin.", "Visual".Colorize(Colors.yellowGreen))),

                new IntegerSlider(MenuDisplayMode.Both, "setting:spin_speed", "LAMP SPIN SPEED")
                    .WithGetter(() => Config.SpinSpeed)
                    .WithSetter((x) => Config.SpinSpeed = x)
                    .LimitedByRange(-500, 500)
                    .WithDefaultValue(50)
                    .WithDescription(string.Format("{0}: Set the lamp spin speed.", "Visual".Colorize(Colors.yellowGreen))),
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "DECORATIVE LAMP", "Settings for the Decorative Lamp mod.");
        }
    }
}