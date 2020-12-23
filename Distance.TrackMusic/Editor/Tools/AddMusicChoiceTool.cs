using Centrifuge.Distance.EditorTools.Attributes;
using LevelEditorActions;
using LevelEditorTools;
using UnityEngine;

namespace Distance.TrackMusic.Editor.Tools
{
	[EditorTool]
	public class AddMusicChoiceTool : InstantTool
	{
		public static ToolInfo info_ = new ToolInfo("Add Music Choice", "", ToolCategory.Others, ToolButtonState.Invisible, false);
		public override ToolInfo Info_ => info_;

		public static GameObject[] Target = new GameObject[0];

		public static void Register()
		{
			G.Sys.LevelEditor_.RegisterTool(info_);
		}

		public override bool Run()
		{
			GameObject[] selected = Target;

			if (selected.Length == 0)
			{
				return false;
			}

			foreach (var obj in selected)
			{
				if (obj.HasComponent<LevelSettings>() || obj.HasComponent<MusicTrigger>() || obj.HasComponent<MusicZone>())
				{
					var listener = obj.GetComponent<ZEventListener>();

					if (listener == null)
					{
						var action = new AddMusicChoiceAction(obj);
						action.Redo();
						action.FinishAndAddToLevelEditorActions();
					}
				}
			}

			return true;
		}
	}
}
