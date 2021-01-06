using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignResource
	{
		[JsonProperty]
		public string name;

		[JsonProperty]
		public ResourceType type;

		[JsonProperty]
		public Dictionary<string, string> files;

		[JsonProperty]
		public Dictionary<string, string> linked_resources;
	}
}
