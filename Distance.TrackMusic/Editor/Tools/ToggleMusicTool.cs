using Centrifuge.Distance.EditorTools.Attributes;
using LevelEditorActions;
using LevelEditorTools;

namespace Distance.TrackMusic.Editor.Tools
{
	[EditorTool]
	public class ToggleMusicTool : InstantTool
	{
		public static ToolInfo info_ = new ToolInfo("Toggle Custom Music", "", ToolCategory.File, ToolButtonState.Button, false);
		public override ToolInfo Info_ => info_;

		public static void Register()
		{
			G.Sys.LevelEditor_.RegisterTool(info_);
		}

		public override bool Run()
		{
			var action = new ToggleMusicAction();
			action.Redo();
			action.FinishAndAddToLevelEditorActions();
			return true;
		}
	}
}
