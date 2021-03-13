#pragma warning disable CS0067, CS0649
#if APP
using Eto.Forms;
#endif
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignPlaylist : INotifyPropertyChanged
	#if APP
		, IListItem
	#endif
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[JsonProperty("guid")]
		public string Guid { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("display_in_sprint")]
		public bool DisplayInSprint { get; set; }

		[JsonProperty("display_in_campaign")]
		public bool DisplayInCampaign { get; set; }

		[JsonProperty("levels")]
		public List<CampaignLevel> Levels { get; set; }

		public CampaignPlaylist()
		{
			Levels = new List<CampaignLevel>();
		}

#if APP
		[JsonIgnore]
		public string Text
		{
			get => Name?.Length > 0 ? Name : Constants.PLAYLIST_NO_NAME; 
			set => Name = value;
		}

		[JsonIgnore]
		public string Key => Guid;

#endif
	}
}
