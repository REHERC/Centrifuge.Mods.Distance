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

		public IEnumerator Run(Task.Status status)
		{
			foreach (LoaderTask task in loader)
			{
				yield return Task.Wrap(task.Run(status, loader));
			}
		}

		public abstract class LoaderTask
		{
			public abstract IEnumerator Run(Task.Status status, CampaignLoader loader);
		}

		public sealed class CampaignLoader : IEnumerable<LoaderTask>
		{
			private readonly Queue<LoaderTask> tasks;

			public CampaignWorkspaceSetup WorkspaceSetup { get; }

			public CampaignListing Listing { get; }

			public CampaignExtractor Extractor { get; }

			public CampaignImporter Importer { get; }

			public CampaignLoader()
			{
				tasks = new Queue<LoaderTask>();

				tasks.Enqueue(WorkspaceSetup = new CampaignWorkspaceSetup());
				tasks.Enqueue(Listing = new CampaignListing());
				tasks.Enqueue(Extractor = new CampaignExtractor());
				tasks.Enqueue(Importer = new CampaignImporter());
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
