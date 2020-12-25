using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using System.Linq;
using UnityEngine;

namespace Distance.HalloweenSeasonalFeatures
{
	[ModEntryPoint("com.github.reherc/Distance.HalloweenSeasonalFeatures")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public FileSystem Data { get; set; }

		public Log Logger { get; set; }

		public ConfigurationLogic Config { get; private set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;

			Data = new FileSystem();
			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigurationLogic>();

			RuntimePatcher.AutoPatch();
			CreateSettingsMenu();
		}
		public void LateInitialize(IManager _)
		{
			GameObject GenerateRandomPumpkinsPrefab = Resource.GetResource<GameObject>("GenerateRandomPumpkins");

			GameObject GenerateRandomPumpkinsObject = Instantiate(GenerateRandomPumpkinsPrefab);
			GenerateRandomPumpkinsObject.name = nameof(GenerateRandomPumpkins);

			DontDestroyOnLoad(GenerateRandomPumpkinsObject);

			GenerateRandomPumpkinsObject.GetComponent<GenerateRandomPumpkins>();
		}

		public void CreateSettingsMenu()
		{
			MenuTree menu = new MenuTree("halloweenfeatures#main.menu", "HALLOWEEN FEATURES");

			Menus.AddNew(MenuDisplayMode.Both, menu, "Settings for the Halloween Seasonal Features mod");
		}
	}
}
