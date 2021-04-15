using Centrifuge.Distance.Game;
using Events.MainMenu;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using Serializers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
			/*SteamworksUGC ugc = G.Sys.SteamworksManager_.UGC_;

			FileSystem files = new FileSystem();

			string filePath = Path.Combine(files.VirtualFileSystemRoot, "obscuration");

			SteamworksUGC.LevelStuff wItem = new SteamworksUGC.LevelStuff
			(
				"Obscuration [preview]",
				"obscuration",
				"MyLevels/obscuration-preview.bytes",
				"This is a level I made for a campaign project that I will release in the future, meanwhile here's a very short preview :D",
				new List<string>()
				{
					"Level"
				}
			);

			MessageBox.Create(filePath, "UPLOAD")
			.SetButtons(Centrifuge.Distance.Data.MessageButtons.YesNo)
			.OnConfirm(() =>
			{
				ugc.SteamFileWrite("obscuration", filePath);
				ugc.SteamFileWrite("obscuration.png", filePath + ".png");

				ugc.AddTask(new SteamworksUGC.PublishWorkshopFileTask(ugc, wItem, SteamworksManager.AppID_, (success, steam_ugc) => { }));

			})
			.Show();*/

			//Task.Run(MyTask);
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