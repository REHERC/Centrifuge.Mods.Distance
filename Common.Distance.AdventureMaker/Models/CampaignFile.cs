using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public partial class CampaignFile
	{
		[JsonProperty("metadata")]
		public CampaignMetadata metadata;

		[JsonProperty("data")]
		public CampaignData data;

		public CampaignFile()
		{
			metadata = new CampaignMetadata();
			data = new CampaignData();
		}
	}
}
