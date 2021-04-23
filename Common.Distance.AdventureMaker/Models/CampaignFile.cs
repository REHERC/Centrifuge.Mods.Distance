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

		[JsonProperty("metadata", Required = Required.Always)]
		public CampaignMetadata Metadata { get; set; }

		[JsonProperty("data", Required = Required.Always)]
		public CampaignData Data { get; set; }

		public CampaignFile()
		{
			Metadata = new CampaignMetadata();
			Data = new CampaignData();
		}
	}
}
