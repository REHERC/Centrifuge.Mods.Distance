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

		public CampaignData()
		{
			playlists = new List<CampaignPlaylist>();
		}
	}
}
