using Centrifuge.Distance.EditorTools.Attributes;
using LevelEditorActions;
using LevelEditorTools;
using UnityEngine;

namespace Distance.TrackMusic.Editor.Tools
{
	[EditorTool]
	public class AddMusicTrackTool : InstantTool
	{
		public static ToolInfo info_ = new ToolInfo("Add Music Track", "", ToolCategory.Edit, ToolButtonState.Button, false);

		public override ToolInfo Info_ => info_;

		public static void Register()
		{
			G.Sys.LevelEditor_.RegisterTool(info_);
		}

		public override bool Run()
		{
			LevelEditor editor = G.Sys.LevelEditor_;

			var action = new AddMusicTrackAction();

			GameObject gameObject = action.CreateTrack();

			editor.ClearSelectedList(true);

			editor.SelectObject(gameObject);

			action.FinishAndAddToLevelEditorActions();

			return true;
		}
	}
}
