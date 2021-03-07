using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(SelectionGroupData), MethodType.Constructor, new Type[2] { typeof(IEnumerable<GameObject>), typeof(GameObject) })]
	internal static class SelectionGroupData__ctor_
	{
		[HarmonyPostfix]
		internal static void Postfix(ref Vector3 ___position_, ref Quaternion ___rotation_, IEnumerable<GameObject> selectedObjects, GameObject activeObject)
		{
			___position_ = Vector3.zero;
			___rotation_ = activeObject ? activeObject.transform.rotation : Quaternion.identity;

			int num = 0;

			foreach (GameObject selectedObject in selectedObjects)
			{
				if (selectedObject)
				{
					___position_ += selectedObject.transform.position;
				}

				++num;
			}

			___position_ /= num;

			if (!___position_.IsValid())
			{
				___position_ = Vector3.zero;
			}
		}
	}
}
