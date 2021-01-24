using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	public abstract partial class CampaignResource
	{
		public class Texture : CampaignResource
		{
			[JsonProperty]
			public override ResourceType resource_type => ResourceType.Texture;

			[JsonProperty]
			public string file;
		}
	}
}
