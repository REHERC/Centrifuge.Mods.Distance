using Newtonsoft.Json;

namespace App.AdventureMaker.Core
{
	public class AppSettings
	{
		public const string SETTINGS_FILE_NAME = "settings.json";

		#region Static
		[JsonIgnore]
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
		#region General
		[JsonProperty("open_last_project_on_startup")]
		public bool OpenLastProject { get; set; } = false;
		#endregion
		#region Preview Mode Settings
		[JsonProperty("preview_mode_run_method")]
		public int PreviewModeRunMethod { get; set; } = 0;

		[JsonProperty("game_executable")]
		public string GameExe { get; set; } = string.Empty;

		[JsonProperty("enable_rcon")]
		public bool EnableRcon { get; set; } = false;
		#endregion
		#endregion
	}
}
