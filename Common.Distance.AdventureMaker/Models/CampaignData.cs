using Distance.AdventureMaker.Common.Models.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignData
	{
		[JsonProperty]
		public List<CampaignPlaylist> playlists;
		
		[JsonProperty]
		public List<CampaignResource> resources;

		public CampaignData()
		{
			playlists = new List<CampaignPlaylist>();
			resources = new List<CampaignResource>();
		}
	}
}
