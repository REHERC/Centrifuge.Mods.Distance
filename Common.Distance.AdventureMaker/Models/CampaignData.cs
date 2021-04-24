#pragma warning disable CS0067
using Distance.AdventureMaker.Common.Models.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignData : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[JsonProperty("playlists")]
		public List<CampaignPlaylist> Playlists { get; set; }

		[JsonProperty("resources")]
		public List<CampaignResource> Resources { get; set; }

		public CampaignData()
		{
			Playlists = new List<CampaignPlaylist>();
			Resources = new List<CampaignResource>();
		}
	}
}