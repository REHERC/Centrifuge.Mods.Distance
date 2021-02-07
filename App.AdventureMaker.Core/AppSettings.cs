using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace App.AdventureMaker.Core
{
	public class AppSettings
	{
		public const string SETTINGS_FILE_NAME = "settings.json";

		#region Static
		[Newtonsoft.Json.JsonIgnore]
		public static AppSettings Instance { get; set; }

		static AppSettings()
		{
			Instance = Json.GetOrCreate(SETTINGS_FILE_NAME, new AppSettings());
		
		}

		public static void Save()
		{
			Json.Save(SETTINGS_FILE_NAME, Instance);
		}
		#endregion

		#region Instance
		[JsonProperty("preview_mode")]
		public int PreviewMode { get; set; } = 0;

		[JsonProperty("game_executable")]
		public string GameExe { get; set; } = string.Empty;
		#endregion
	}
}
