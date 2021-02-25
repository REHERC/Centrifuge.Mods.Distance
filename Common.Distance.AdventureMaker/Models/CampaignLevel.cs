using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignLevel
	{
		[JsonProperty("name")]
		public string name;

		[JsonProperty("resource_id")]
		public string resource_id;

		#region Intro Title
		[JsonProperty("transition")]
		public LevelTransitionType transition;

		[JsonProperty("title")]
		public string title;

		[JsonProperty("title_small")]
		public string title_small;
		#endregion

		#region Loading Screen
		[JsonProperty("loading_background")]
		public string loading_background;

		[JsonProperty("override_loading_text")]
		public bool override_loading_text;

		[JsonProperty("loading_text")]
		public string loading_text;
		#endregion

		#region Gameplay Properties

		#endregion
	}
}
