using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.NoEditorNumberLimits
{
	[ModEntryPoint("com.plasmawario/Distance.NoEditorNumberLimits")]
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

			Logger.Info("NoEditorNumberLimits loaded");
			Logger.Info(":atprtsd:");
			Logger.Info("ps: i totally did this with no help");
			Logger.Info("pps: i can confirm ;)");
			Logger.Info("\t\t- Plasmawario, 2020");

			RuntimePatcher.AutoPatch();
		}
	}
}