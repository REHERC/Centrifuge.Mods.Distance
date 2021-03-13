#pragma warning disable CS0067
using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignLevel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[JsonProperty("guid")]
		public string Guid { get; set; }

		[JsonProperty("name")]
		public string Name;

		[JsonProperty("resource_id")]
		public string ResourceId { get; set; }

		#region Intro Title
		[JsonProperty("transition")]
		public LevelTransitionType Transition { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("title_small")]
		public string TitleSmall { get; set; }
		#endregion

		#region Loading Screen
		[JsonProperty("loading_background")]
		public string LoadingBackground { get; set; }

		[JsonProperty("override_loading_text")]
		public bool OverrideLoadingText { get; set; }

		[JsonProperty("loading_text")]
		public string LoadingText { get; set; }

		#endregion

		#region Gameplay Properties

		#endregion
	}
}
