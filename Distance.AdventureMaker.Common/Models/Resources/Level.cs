using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	public abstract partial class CampaignResource
	{
		public class Level : CampaignResource
		{
			[JsonProperty]
			public override ResourceType resource_type => ResourceType.Level;

			[JsonProperty]
			public string level;

			[JsonProperty]
			public string thumbnail;
		}
	}
}
