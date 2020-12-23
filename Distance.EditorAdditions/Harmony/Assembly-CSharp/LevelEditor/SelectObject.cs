using HarmonyLib;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(LevelEditor), "SelectObject")]
	internal static class LevelEditor__SelectObject
	{
		[HarmonyPrefix]
		internal static bool Prefix(LevelEditor __instance, ref bool __result, ref GameObject newObj)
		{
			if (newObj is null)
			{
				Debug.LogWarning("Trying to select a null object");
				__result = false;
				return false;
			}

			if (!newObj.transform.IsRoot() && false)
			{
				Debug.LogWarning("Trying to select a child object");
				__result = false;

				return false;
			}

			if (!__instance.selectedObjects_.Contains(newObj))
			{
				LevelLayer layerOfObject = __instance.workingLevel_.GetLayerOfObject(newObj);

				if (layerOfObject?.Frozen_ == false)
				{
					__instance.AddObjectToSelectedList(newObj);
					__result = true;

					return false;
				}
			}
			else
			{
				__instance.SetActiveObject(newObj);
			}

			__result = false;

			return false;
		}
	}
}
