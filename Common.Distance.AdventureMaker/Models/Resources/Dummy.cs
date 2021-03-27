#pragma warning disable IDE1006
using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	public abstract partial class CampaignResource
	{
		public class Dummy : CampaignResource
		{
			[JsonProperty("resource_type")]
			public override ResourceType resource_type => ResourceType.None;

			#if APP
			[JsonIgnore]
			public override int dependencies_count => 0;
			#endif

			public Dummy()
			{
				file = "<None>";
			}
		}
	}
}
