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

		[JsonProperty("guid", Required = Required.Always)]
		public string Guid { get; set; }

		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }

		[JsonProperty("description", Required = Required.Always)]
		public string Description { get; set; }

		[JsonProperty("icon", Required = Required.Always)]
		public string Icon { get; set; }

		[JsonProperty("display_in_sprint", Required = Required.Always)]
		public bool DisplayInSprint { get; set; }

		[JsonProperty("display_in_campaign", Required = Required.Always)]
		public bool DisplayInCampaign { get; set; }

		[JsonProperty("levels", Required = Required.Always)]
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
