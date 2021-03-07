#pragma warning disable IDE1006

using Distance.AdventureMaker.Common.Enums;
using JsonSubTypes;
using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	[Serializable]
	[JsonConverter(typeof(JsonSubtypes), "resource_type")]
	[JsonSubtypes.KnownSubType(typeof(Texture), ResourceType.Texture)]
	[JsonSubtypes.KnownSubType(typeof(Level), ResourceType.Level)]
	public abstract partial class CampaignResource
	{
		[JsonProperty("guid")]
		public string guid;

		[JsonProperty("file")]
		public string file;

		[JsonProperty("resource_type")]
		public virtual ResourceType resource_type { get; }

		#if APP
		[JsonIgnore]
		public abstract int dependencies_count { get; }
		#endif

		protected CampaignResource()
		{
			guid = Guid.NewGuid().ToString();
		}
	}
}
