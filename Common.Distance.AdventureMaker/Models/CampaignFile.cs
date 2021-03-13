#pragma warning disable CS0067
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public partial class CampaignFile : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[JsonProperty("metadata")]
		public CampaignMetadata Metadata { get; set; }

		[JsonProperty("data")]
		public CampaignData Data { get; set; }

		public CampaignFile()
		{
			Metadata = new CampaignMetadata();
			Data = new CampaignData();
		}
	}
}
