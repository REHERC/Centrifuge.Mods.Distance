using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	[Serializable]
	public abstract partial class CampaignResource
	{
		[JsonProperty]
		public string name;

		[JsonProperty]
		public ResourceType type;
	}
}
