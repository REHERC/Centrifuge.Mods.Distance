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
		[JsonProperty]
		public string guid;

		[JsonProperty]
		public string name;

		[JsonProperty]
		public string description;

		[JsonProperty]
		public string icon;

		[JsonProperty]
		public bool display_in_sprint;

		[JsonProperty]
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
