using UnityEngine;

namespace Distance.ChristmasSeasonalFeatures
{
	public static partial class InternalResources
	{
		public static class Constants
		{
			public const string LOADINGSCREENS_FOLDER = "Wallpapers";

			public static string[] LOADINGSCREENS_FILES => new string[]
			{
				"loadingscreen-boosting-system.png",
				"loadingscreen-flying-system.png",
				"loadingscreen-jumping-system.png",
				"loadingscreen-quantum-lock-devices.png",
				"loadingscreen-quantum-capsules.png",
				"loadingscreen-road-division-equipment.png",
				"loadingscreen-road-smoothing-equipment.png",
				"loadingscreen-teleport-augmentation.png"
			};

			public static Color CHEAT_VISUAL_COLOR => Colors.yellowGreen;
			public static string CHEAT_VISUAL => "Visual".Colorize(CHEAT_VISUAL_COLOR);

			public const string REINDEER_COSMETIC = "RefractorReindeer";
		}
	}
}
