using Centrifuge.Distance.EditorTools.Attributes;
using LevelEditorActions;
using LevelEditorTools;
using System.Linq;
using UnityEngine;

namespace Distance.TrackMusic.Editor.Tools
{
	[EditorTool]
	public class RemoveMusicChoiceTool : InstantTool
	{
		public static ToolInfo info_ = new ToolInfo("Remove CustomMusic", "", ToolCategory.Others, ToolButtonState.Invisible, false);

		public override ToolInfo Info_ => info_;

		public static void Register()
		{
			G.Sys.LevelEditor_.RegisterTool(info_);
		}

		public ZEventListener[] components;

		public void SetComponents(Component[] componentsP)
		{
			components = componentsP.Cast<ZEventListener>().ToArray();
		}

		public override bool Run()
		{
			if (components == null)
			{
				return false;
			}

			ZEventListener[] selected = components;

			if (selected.Length == 0)
			{
				return false;
			}

			foreach (var obj in selected)
			{
				var action = new RemoveMusicChoiceAction(obj.gameObject, obj);
				action.Redo();
				action.FinishAndAddToLevelEditorActions();
			}

			return true;
		}
	}
}
