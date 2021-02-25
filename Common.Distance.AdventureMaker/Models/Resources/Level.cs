using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	public abstract partial class CampaignResource
	{
		public class Level : CampaignResource
		{
			[JsonProperty("resource_type")]
			public override ResourceType resource_type => ResourceType.Level;

			[JsonProperty("thumbnail")]
			public string thumbnail;

			#if APP
			public override int dependencies_count => 1;
			#endif
		}
	}
}
