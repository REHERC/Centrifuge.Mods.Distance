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

		[JsonProperty("guid", Required = Required.Always)]
		public string Guid { get; set; }

		[JsonProperty("version", Required = Required.Always)]
		public long Version { get; set; }

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; set; }

		[JsonProperty("description", Required = Required.Always)]
		public string Description { get; set; }

		[JsonProperty("author", Required = Required.Always)]
		public string Author { get; set; }

		[JsonProperty("contact", Required = Required.Always)]
		public string Contact { get; set; }

		[JsonProperty("development_status", Required = Required.Always)]
		public DevelopmentStatus DevelopmentStatus { get; set; }
	}
}