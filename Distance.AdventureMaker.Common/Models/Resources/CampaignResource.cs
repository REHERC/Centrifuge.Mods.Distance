#pragma warning disable IDE1006

using Distance.AdventureMaker.Common.Enums;
using JsonSubTypes;
using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	[Serializable]
	[JsonConverter(typeof(JsonSubtypes), "resource_type")]
	[JsonSubtypes.KnownSubType(typeof(CampaignResource.Texture), ResourceType.Texture)]
	[JsonSubtypes.KnownSubType(typeof(CampaignResource.Level), ResourceType.Level)]
	public abstract partial class CampaignResource
	{
		[JsonProperty]
		public string guid;

		[JsonProperty]
		public virtual ResourceType resource_type { get; }
	}
}
