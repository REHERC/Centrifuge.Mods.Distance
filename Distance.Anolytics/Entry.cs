using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System.IO;
using UnityEngine;

namespace Distance.Anolytics
{
    [ModEntryPoint("eu.vddcore/Distance.Anolytics")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance;

        public IManager Manager { get; set; }

        public Log Logger { get; set; }

        public ConfigurationLogic Config { get; set; }
        
        public void Initialize(IManager manager)
        {
            Instance = this;
            Manager = manager;

            Logger = LogManager.GetForCurrentAssembly();

            Config = gameObject.AddComponent<ConfigurationLogic>();

            CreateSettingsMenu();

            RuntimePatcher.AutoPatch();
        }

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.anolytics", "Anolytics Settings")
            {
                new CheckBox(MenuDisplayMode.Both, "setting:shutdown_analytics", "DISABLE ANALYTICS")
                .WithGetter(() => Config.ShutDownAnalytics)
                .WithSetter((x) => Config.ShutDownAnalytics = x)
                .WithDescription("Disables connection with Google analytics services.\n(Avoids requests to \"https://www.google-analytics.com/__utm.gif?\")"),

                new CheckBox(MenuDisplayMode.Both, "setting:log_analytics", "LOG ANALYTICS ACTIVITY")
                .WithGetter(() => Config.LogAnalytics)
                .WithSetter((x) => Config.LogAnalytics = x)
                .WithDescription("Outputs analytics events to the console."),

                new CheckBox(MenuDisplayMode.Both, "setting:disable_playtest_logging", "DISABLE PLAYTESTING LOG FILE")
                .WithGetter(() => Config.DisablePlaytestingDataLogging)
                .WithSetter((x) => Config.DisablePlaytestingDataLogging = x)
                .WithDescription("Blocks file write attempts to \"playtesting_data.txt\"."),

                new ActionButton(MenuDisplayMode.Both, "setting:delete_playtest_log", "DELETE PLAYTEST LOG FILE")
                .WhenClicked(() =>
                {
                    MessageBox.Create("Are you sure you want to delete \"playtesting_data.txt\" ?", "REMOVE FILE")
                    .SetButtons(MessageButtons.YesNo)
                    .OnConfirm(() =>
                    {
                        FileInfo file = new FileInfo(Path.Combine(Application.dataPath, "playtesting_data.txt"));

                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    })
                    .Show();
                })
                .WithDescription("Removes the playtesting log file from the disk.")
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "ANOLYTICS", "Change settings of the Anolytics mod.");
        }
    }
}