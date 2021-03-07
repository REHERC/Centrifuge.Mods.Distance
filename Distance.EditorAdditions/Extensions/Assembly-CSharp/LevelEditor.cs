#pragma warning disable RCS1110
using UnityEngine;

public static class LevelEditorExtensions
{
	public static void ClearAndSelect(this LevelEditor editor, GameObject prefab)
	{
		editor.ClearSelectedList(true);
		editor.SelectObject(prefab);
	}
}