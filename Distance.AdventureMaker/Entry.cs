using Centrifuge.Distance.Game;
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

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public ApplicationArguments StartParameters { get; set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;
			Logger = LogManager.GetForCurrentAssembly();
			StartParameters = ApplicationArguments.Parse();
			
			RuntimePatcher.AutoPatch();

			Initialized.Subscribe(OnMainMenuInitialized);
		}

		private void OnMainMenuInitialized(Initialized.Data data)
		{
			//MessageBox.Create($"Arguments:\npreview: {StartParameters.IsPreviewMode}\nfile: {StartParameters.CampaignFile}\nrcon: {StartParameters.RConPort}").Show();
		}
	}
}