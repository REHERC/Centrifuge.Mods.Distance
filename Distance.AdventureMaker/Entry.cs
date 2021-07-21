using Distance.AdventureMaker.Loader;
using Distance.AdventureMaker.Scripts.Managers.Campaigns;
using Events.MainMenu;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.AdventureMaker
{
	[ModEntryPoint("com.github.reherc/Distance.AdventureMaker")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; private set; }

		public Log Logger { get; private set; }

		public ApplicationArguments StartParameters { get; private set; }

		private CampaignLoaderLogic CampaignLoader { get; set; }

		internal CampaignManager CampaignManager { get; private set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;
			Logger = LogManager.GetForCurrentAssembly();

			CampaignLoader = gameObject.AddComponent<CampaignLoaderLogic>();
			CampaignManager = gameObject.AddComponent<CampaignManager>();

			StartParameters = ApplicationArguments.Parse();

			RuntimePatcher.AutoPatch();

			Initialized.Subscribe(OnMainMenuInitialized);
		}

		private void OnMainMenuInitialized(Initialized.Data data)
		{
			//Task.Run(CampaignLoader);
			CampaignLoader.Run();
			//MessageBox.Create($"Arguments:\npreview: {StartParameters.IsPreviewMode}\nfile: {StartParameters.CampaignFile}\nrcon: {StartParameters.RConPort}").Show();

			Initialized.Unsubscribe(OnMainMenuInitialized);
		}
	}
}