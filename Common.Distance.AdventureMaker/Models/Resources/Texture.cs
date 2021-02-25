using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	public abstract partial class CampaignResource
	{
		public class Texture : CampaignResource
		{
			[JsonProperty("resource_type")]
			public override ResourceType resource_type => ResourceType.Texture;

			#if APP
			public override int dependencies_count => 0;
			#endif
		}
	}
}
