using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.EditorAdditions
{
	public static class EditorUtil
	{
		public static Dictionary<ToolCategory, int> CategorySort = new Dictionary<ToolCategory, int>();
		public static Dictionary<int, GameObject> QuickSelectMemory = new Dictionary<int, GameObject>();

		public static void SetQuickMemory(int index, GameObject instance)
		{
			QuickSelectMemory[index] = instance;
		}

		public static GameObject GetQuickMemory(int index)
		{
			if (QuickSelectMemory.ContainsKey(index))
			{
				return QuickSelectMemory[index];
			}
			else
			{
				return null;
			}
		}

		public static void ClearQuickMemory()
		{
			QuickSelectMemory.Clear();
		}

		public static void Inspect(GameObject Target)
		{
			NGUIObjectInspectorTab Inspector = Object.FindObjectOfType<NGUIObjectInspectorTab>();

			var Editor = G.Sys.LevelEditor_;

			if (Editor && Inspector)
			{
				Editor.ClearOutlines();
				Editor.SelectedObjects_.Clear();
				Editor.SelectedObjects_.Add(Target);
				Editor.AddOutline(Target.Root());
				Editor.AddOutline(Target);
				Editor.UpdateOutlines();

				Editor.activeObject_ = Target;
				Inspector.targetObject_ = Target;
				Inspector.ClearComponentInspectors();
				Inspector.InitComponentInspectorsOnTargetObject();
				Inspector.InitAddComponentButton();
				Inspector.propertiesNeedToBeUpdated_ = false;
				Inspector.objectNameLabel_.text = Inspector.targetObject_.GetDisplayName();
			}
		}

		public static void InspectRoot()
		{
			var Editor = G.Sys.LevelEditor_;
			GameObject selection = Editor.activeObject_;

			if (Editor && selection)
			{
				Inspect(selection.Root());
				Editor.ClearSelectedList();
				Editor.SelectObject(selection.Root());
			}
		}

		public static bool IsSelectionRoot()
		{
			GameObject selection = G.Sys.LevelEditor_.activeObject_;

			if (selection)
			{
				return selection.transform.IsRoot();
			}
			else
			{
				return true;
			}
		}

		public static void PrintToolInspectionStackError()
		{
			MessageBox.Create("You can't run this tool while inspecting a group stack.\nPlease select a root level object.", "ERROR")
				.SetButtons(MessageButtons.Ok)
				.Show();
		}
	}
}
