using Centrifuge.Distance.Game;
using System.Collections;
using static Distance.AdventureMaker.Loader.CampaignLoaderLogic;

namespace Distance.AdventureMaker.Loader.Steps
{
	public class CampaignExtractor : LoaderTask
	{
		public CampaignExtractor(CampaignLoader loader) : base(loader)
		{
		}

		public override IEnumerator Run(Task.Status status)
		{
			foreach (var item in loader.Listing)
			{
				Mod.Instance.Logger.Info($"[{item.Value.source}]\t {item.Key} : {item.Value.path.FullName}");
			}

			yield break;
		}
	}
}
