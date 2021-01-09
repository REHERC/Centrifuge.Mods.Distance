#pragma warning disable CS0649

using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public partial class CampaignMetadata
	{
		[JsonProperty]
		public string guid;

		[JsonProperty]
		public string title;

		[JsonProperty]
		public string description;

		[JsonProperty]
		public string author;

		[JsonProperty]
		public string contact;

		[JsonProperty]
		public bool dev;
	}
}
