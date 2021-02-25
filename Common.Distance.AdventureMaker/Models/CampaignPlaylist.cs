#pragma warning disable CS0649

#if APP
using App.AdventureMaker.Core;
using Eto.Forms;
#endif
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignPlaylist
	#if APP
		: IListItem
	#endif
	{
		[JsonProperty("guid")]
		public string guid;

		[JsonProperty("name")]
		public string name;

		[JsonProperty("description")]
		public string description;

		[JsonProperty("icon")]
		public string icon;

		[JsonProperty("display_in_sprint")]
		public bool display_in_sprint;

		[JsonProperty("display_in_campaign")]
		public bool display_in_campaign;

		[JsonProperty("levels")]
		public List<CampaignLevel> levels;

		public CampaignPlaylist()
		{
			levels = new List<CampaignLevel>();
		}

		#if APP
		[JsonIgnore]
		public string Text 
		{ 
			get => name?.Length > 0 ? name : Constants.PLAYLIST_NO_NAME; 
			set => name = value; 
		}

		[JsonIgnore]
		public string Key => guid;
		#endif
	}
}
