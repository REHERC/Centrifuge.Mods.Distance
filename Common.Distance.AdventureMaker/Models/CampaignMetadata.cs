#pragma warning disable CS0649

using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public partial class CampaignMetadata
	{
		[JsonProperty("guid")]
		public string guid;

		[JsonProperty("title")]
		public string title;

		[JsonProperty("description")]
		public string description;

		[JsonProperty("author")]
		public string author;

		[JsonProperty("contact")]
		public string contact;

		[JsonProperty("development_status")]
		public DevelopmentStatus development_status;
	}
}