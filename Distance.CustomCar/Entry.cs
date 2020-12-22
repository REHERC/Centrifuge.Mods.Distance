#pragma warning disable IDE0051

using Distance.CustomCar.Data.Car;
using Distance.CustomCar.Data.Colors;
using Distance.CustomCar.Data.Error;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using System;
using System.IO;
using UnityEngine;

namespace Distance.CustomCar
{
	[ModEntryPoint("com.github.larnin/CustomCar")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public static int DefaultCarCount { get; private set; }

		public static int ModdedCarCount => Instance.CarFactory.Builder.Count;

		public static int TotalCarCount => DefaultCarCount + ModdedCarCount;

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public FileSystem Files { get; set; }
		
		public ErrorList Errors { get; set; }

		public CarFactory CarFactory { get; set; }

		public ProfileDataLogic ProfileData { get; set; }

		public ProfileCarColors ProfileCarColors { get; set; }


		public void Initialize(IManager manager)
		{
			Instance = this;
			Manager = manager;

			ProfileData = gameObject.AddComponent<ProfileDataLogic>();
			ProfileCarColors = new ProfileCarColors(ProfileData);

			Logger = LogManager.GetForCurrentAssembly();
			Files = new FileSystem();

			Errors = new ErrorList(Logger);

			CarFactory = new CarFactory(Errors);

			RuntimePatcher.AutoPatch();
		}


		public void LateInitialize(IManager _)
		{
			DefaultCarCount = G.Sys.ProfileManager_.CarInfos_.Length;
		}

		public DirectoryInfo GetLocalFolder(string dir)
		{
			return new DirectoryInfo(Path.GetDirectoryName(Path.Combine(Files.RootDirectory, dir + (dir.EndsWith($"{Path.DirectorySeparatorChar}") ? string.Empty : $"{Path.DirectorySeparatorChar}"))));
		}

		private void OnEnable()
		{
			Events.MainMenu.Initialized.Subscribe(OnMainMenuInitialized);
		}

		private void OnDisable()
		{
			Events.MainMenu.Initialized.Unsubscribe(OnMainMenuInitialized);
		}

		public void Load()
		{
			CarFactory.LoadAssets();
		}

		private bool displayError_ = true;

		private void OnMainMenuInitialized(Events.MainMenu.Initialized.Data data)
		{
			try
			{
				CarFactory.MakeCars();
				CarFactory.RegisterAll();

				ProfileCarColors.LoadColors();
			}
			catch (Exception error)
			{
				Errors.Add("An error occured while trying to load cars assets.", "Initialization");
				Logger.Exception(error);
			}

			if (displayError_)
			{
				displayError_ = false;
				Errors.Show(true);
			}
		}
	}
}