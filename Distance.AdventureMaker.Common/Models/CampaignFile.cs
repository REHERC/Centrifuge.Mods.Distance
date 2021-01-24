using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public partial class CampaignFile
	{
		[JsonProperty]
		public CampaignMetadata metadata;

		[JsonProperty]
		public CampaignData data;

		public CampaignFile()
		{
			metadata = new CampaignMetadata();
			data = new CampaignData();
		}
	}
}
