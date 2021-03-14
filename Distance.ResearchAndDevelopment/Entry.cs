using Centrifuge.Distance.Game;
using Events.MainMenu;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System.Collections;
using UnityEngine;

namespace Distance.ResearchAndDevelopment
{
	[ModEntryPoint("com.github.reherc/Distance.ResearchAndDevelopment")]
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

			RuntimePatcher.AutoPatch();

			Initialized.Subscribe(OnMainMenuInitialized);
		}

		private void OnMainMenuInitialized(Initialized.Data data)
		{
			Task.Run(MyTask);
		}

		private IEnumerator MyTask(Task.Status status)
		{
			const int max = 500;

			status.SetText("Setting up...");
			status.SetProgress(0, 1);

			yield return Task.Wait(1.5f);

			for (uint i = 1; i <= max; ++i)
			{
				status.SetText($"Running task {i} of {max}...");
				status.SetProgress(i, max);

				yield return Task.Wait(0.05f);
			}

			status.SetText("Finishing...");
			status.SetProgress(1, 1);

			yield return Task.Wait(2.0f);
		}
	}
}