using Centrifuge.Distance.Game;
using Distance.AdventureMaker.Loader.Steps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.AdventureMaker.Loader
{
	public class CampaignLoaderLogic : MonoBehaviour
	{
		private CampaignLoader loader;

		public void Start()
		{
			loader = new CampaignLoader();
		}

		public void Run()
		{
			foreach (LoaderTask task in loader)
			{
				Task.Run(task);
			}
		}

		public abstract class LoaderTask
		{
			protected readonly CampaignLoader loader;

			protected LoaderTask(CampaignLoader loader)
			{
				this.loader = loader;
			}

			public abstract IEnumerator Run(Task.Status status);

			public static implicit operator Task.TaskDelegate(LoaderTask task)
			{
				return task.Run;
			}
		}

		public sealed class CampaignLoader : IEnumerable<LoaderTask>
		{
			private readonly Queue<LoaderTask> tasks;

			public CampaignWorkspaceSetup Workspace { get; }

			public CampaignListing Listing { get; }

			public CampaignExtractor Extractor { get; }

			public CampaignImporter Importer { get; }

			public CampaignLoader()
			{
				tasks = new Queue<LoaderTask>();

				tasks.Enqueue(Workspace = new CampaignWorkspaceSetup(this));
				tasks.Enqueue(Listing = new CampaignListing(this));
				tasks.Enqueue(Extractor = new CampaignExtractor(this));
				tasks.Enqueue(Importer = new CampaignImporter(this));
			}

			public IEnumerator<LoaderTask> GetEnumerator()
			{
				return tasks.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}
