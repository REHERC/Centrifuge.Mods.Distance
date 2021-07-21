using Centrifuge.Distance.Game;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using System.Collections;
using System.IO;
using static Distance.AdventureMaker.Loader.CampaignLoaderLogic;

namespace Distance.AdventureMaker.Loader.Steps
{
	public class CampaignImporter : LoaderTask
	{
		public CampaignImporter(CampaignLoader loader) : base(loader)
		{
		}

		public override IEnumerator Run(Task.Status status)
		{
			yield break;
			status.SetText("Loading campaigns...");

			foreach (DirectoryInfo campaignPath in loader.Extractor)
			{
				CampaignFile campaign = Json.Load<CampaignFile>(Path.Combine(campaignPath.FullName, "project.json"));
				//Mod.Instance.Logger.Warning($"{campaign.Metadata.Title} by {campaign.Metadata.Author}");

				foreach (var resource in campaign.GetResources(ResourceType.Level))
				{
					CampaignResource.Level level = resource as CampaignResource.Level;

					string levelAlias = $"ModdedLevels/{campaign.Metadata.Guid}/{level.guid}.bytes";
					string bytesPath = GetResourceFullPath(campaignPath, level.file);
					string thumbPath = GetResourceFullPath(campaignPath, level.thumbnail);

					Mod.Instance.CampaignManager.Levels.RegisterLevel(levelAlias, bytesPath, thumbPath);
				}
			}
		}

		protected string GetResourceFullPath(DirectoryInfo campaignRoot, string file)
		{
			return Path.Combine(campaignRoot.FullName, "resources/" + file);
		}
	}
}
