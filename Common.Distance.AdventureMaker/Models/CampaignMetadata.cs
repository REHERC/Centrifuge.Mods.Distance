#pragma warning disable CS0067, CS0649
using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignMetadata : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[JsonProperty("guid")]
		public string Guid { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }

		[JsonProperty("contact")]
		public string Contact { get; set; }

		[JsonProperty("development_status")]
		public DevelopmentStatus DevelopmentStatus { get; set; }
	}
}