using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models
{
	[Serializable]
	public class CampaignLevel
	{
		[JsonProperty]
		public string name;

		[JsonProperty]
		public string file;

		#region Intro Title
		[JsonProperty]
		public LevelTransitionType transition;

		[JsonProperty]
		public string title;

		[JsonProperty]
		public string title_small;
		#endregion

		#region Loading Screen
		[JsonProperty]
		public string loading_background;

		[JsonProperty]
		public bool override_loading_text;

		[JsonProperty]
		public string loading_text;
		#endregion

		#region Gameplay Properties

		#endregion
	}
}
