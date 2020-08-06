using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using System.IO;
using UnityEngine;

namespace Distance.CustomWheelHologram
{
    [ModEntryPoint("eu.vddcore/Distance.CustomWheelHologram")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance;

        public IManager Manager { get; set; }

        public Log Logger { get; set; }

        public ConfigurationLogic Config { get; set; }

        public FileSystem FileSystem { get; set; }

        internal FileInfo WheelImage => new FileInfo(Path.Combine(FileSystem.VirtualFileSystemRoot, Config.FileName));

        private Texture _customImage = null;
        public Texture2D CustomImage
        {
            get
            {
                if (!_customImage)
                {
                    if (WheelImage.Exists)
                    {
                        _customImage = Resource.LoadTextureFromFile(WheelImage.FullName, 512, 512);
                    }
                    else
                    {
                        _customImage = null;
                    }
                }

                return _customImage as Texture2D;
            }
        }

        public Texture2D OriginalImage { get; internal set; }

        public void Initialize(IManager manager)
        {
            Instance = this;
            Manager = manager;

            Logger = LogManager.GetForCurrentAssembly();

            Config = gameObject.AddComponent<ConfigurationLogic>();
            FileSystem = new FileSystem();

            CreateSettingsMenu();

            RuntimePatcher.AutoPatch();
        }

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.customwheelhologram", "Wheel Hologram Settings")
            {
                new ActionButton(MenuDisplayMode.Both, "setting:select_image", "SELECT IMAGE")
                .WhenClicked(() =>
                {
                    var dlgOpen = new System.Windows.Forms.OpenFileDialog
                    {
                        Filter = "Image file (*.png, *.jpg, *.jpeg *.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All Files (*.*)|*.*",
                        SupportMultiDottedExtensions = true,
                        RestoreDirectory = true,
                        Title = "Select an image file",
                        CheckFileExists = true,
                        CheckPathExists = true
                    };

                    if (dlgOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FileInfo image = new FileInfo(dlgOpen.FileName);

                        if (image.Exists)
                        {
                            Config.FileName = Path.GetFileName(image.CopyTo(Path.Combine(FileSystem.VirtualFileSystemRoot, Path.GetFileName(image.FullName)), true).FullName);
                            Config.Enabled = true;
                        }
                    }
                })
                .WithDescription("Select the image file displayed on the wheel hologram."),

                new ActionButton(MenuDisplayMode.Both, "setting:reset_image", "RESET IMAGE")
                .WhenClicked(() =>
                {
                    MessageBox.Create("Are you sure you want to reset the hologram to its default image?", "RESET WHEEL IMAGE")
                    .SetButtons(MessageButtons.YesNo)
                    .OnConfirm(() =>
                    {
                        Config.Enabled = false;
                        Config.FileName = string.Empty;

                        if (WheelImage.Exists)
                        {
                            WheelImage.Delete();
                        }
                    });
                })
                .WithDescription("Resets the wheel hologram to the game's default.")
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "CUSTOM WHEEL HOLOGRAM SETTINGS", "Change settings of the Custom Wheel Hologram mod and customize your vehicle's wheel image (works only with vanilla cars).");
        }
    }
}