#pragma warning disable IDE0051

using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Distance.CustomCar.Data.Car;
using Distance.CustomCar.Data.Errors;
using Events.MainMenu;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.CustomCar
{
	[ModEntryPoint("com.github.larnin/CustomCar")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		#region Static Variables
		public static int DefaultCarCount { get; private set; }

		public static int ModdedCarCount => TotalCarCount - DefaultCarCount;

		public static int TotalCarCount { get; private set; }
		#endregion

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public ErrorList Errors { get; set; }

		public ProfileCarColors CarColors { get; set; }

		public ConfigurationLogic Config { get; set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;

			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigurationLogic>();
			Errors = new ErrorList(Logger);

			CarColors = gameObject.AddComponent<ProfileCarColors>();

			CreateSettingsMenu();

			RuntimePatcher.AutoPatch();
		}

		// Equivalent of Spectrum's Initialize call (which happens after GameManager initialization)
		public void LateInitialize(IManager _)
		{
			ProfileManager profileManager = G.Sys.ProfileManager_;
			DefaultCarCount = profileManager.CarInfos_.Length;

			CarInfos carInfos = new CarInfos();
			carInfos.CollectInfos();
			CarBuilder carBuilder = new CarBuilder();
			carBuilder.CreateCars(carInfos);

			TotalCarCount = profileManager.CarInfos_.Length;
			CarColors.LoadAll();

			Errors.Show();
		}

		private void OnEnable()
		{
			Initialized.Subscribe(OnMainMenuLoaded);
		}

		private void OnDisable()
		{
			Initialized.Unsubscribe(OnMainMenuLoaded);
		}

		private bool displayErrors_ = true;

		private void OnMainMenuLoaded(Initialized.Data _)
		{
			if (displayErrors_)
			{
				Errors.Show();
				displayErrors_ = false;
			}
		}

		private void CreateSettingsMenu()
		{
			MenuTree settingsMenu = new MenuTree("menu.mod.customcar", "Custom Cars Settings")
			{
				new CheckBox(MenuDisplayMode.Both, "setting:use_trumpet_horn", "USE TRUMPET HORN")
				.WithGetter(() => Config.UseTrumpetHorn)
				.WithSetter((x) => Config.UseTrumpetHorn = x)
				.WithDescription("Custom car models will use the encryptor horn (the \"doot\" trumpet).")
			};

			Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "CUSTOM CARS", "Settings for the Custom Cars mod.");
		}
	}
}