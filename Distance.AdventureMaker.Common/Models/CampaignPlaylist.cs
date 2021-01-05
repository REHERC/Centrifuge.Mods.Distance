#pragma warning disable CS0649

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignPlaylist
	{
		[JsonProperty]
		public string name;

		[JsonProperty]
		public string descrtiption;

		[JsonProperty]
		public string icon;

		[JsonProperty]
		public List<CampaignLevel> levels;

		public CampaignPlaylist()
		{
			levels = new List<CampaignLevel>();
		}
	}
}
