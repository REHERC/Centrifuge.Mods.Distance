using Fclp;
using System;
using static Distance.AdventureMaker.Common.CommandLine;

namespace Distance.AdventureMaker
{
	[Serializable]
	public class ApplicationArguments
	{
		// RCon => Remote CONsole
		public const int RCON_NO_BROADCAST = -1;

		public bool IsPreviewMode { get; set; }

		public string CampaignFile { get; set; }

		public int RConPort { get; set; }

		public bool RConBroadcast => RConPort != RCON_NO_BROADCAST;

		public static ApplicationArguments Parse()
		{
			var parser = new FluentCommandLineParser<ApplicationArguments>();

			parser  /// /preview
			.Setup(obj => obj.IsPreviewMode)
			.As(CLARG_FLG_PREVIEW_MODE)
			.SetDefault(false);

			parser  /// /campaign:"{file path}"
			.Setup(obj => obj.CampaignFile)
			.As(CLARG_VAL_CAMPAIGN_FILE)
			.SetDefault(string.Empty);

			parser  /// /rcon:{number}
			.Setup(obj => obj.RConPort)
			.As(CLARG_VAL_REMOTE_CONSOLE_PORT)
			.SetDefault(RCON_NO_BROADCAST);

			parser.Parse(Environment.GetCommandLineArgs());

			return parser.Object;
		}
	}
}
