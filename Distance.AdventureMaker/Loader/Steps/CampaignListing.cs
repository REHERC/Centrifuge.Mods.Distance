using Centrifuge.Distance.Game;
using System.Collections;
using static Distance.AdventureMaker.Loader.CampaignLoaderLogic;

namespace Distance.AdventureMaker.Loader.Steps
{
	public class CampaignListing : LoaderTask
	{
		public CampaignListing(CampaignLoader loader) : base(loader)
		{
		}

		public override IEnumerator Run(Task.Status status)
		{
			const int max = 100;

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
