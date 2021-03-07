using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(NGUIObjectInspectorTabAbstract), "CreateComponentInspectorsOnObject")]
	internal static class NGUIObjectInspectorTabAbstract__CreateComponentInspectorsOnObject
	{
		[HarmonyPrefix]
		internal static bool Prefix(NGUIObjectInspectorTabAbstract __instance, ref DontInspectComponents.Set ignoreList, ref bool objectSupportsUndo, ref GameObject obj)
		{
			bool is_inspected = obj.Equals(__instance.targetObject_);
			bool is_object_root = obj.transform.IsRoot() || is_inspected;

			if (is_object_root && (!ignoreList.ShouldBeIgnored(obj.transform) || is_inspected))
			{
				__instance.CreateISerializableInspector(ComponentSerializeWrapper.Create(obj.transform, true), objectSupportsUndo);
			}

			Group group = obj.GetComponent<Group>();

			bool inspectChildren = group == null;
			bool can_inspect_mateirals = is_object_root && (group == null || group.inspectChildren_ != Group.InspectChildrenType.None);

			if (group)
			{
				__instance.CreateISerializableInspector(group, objectSupportsUndo);
			}

			if (can_inspect_mateirals)
			{
				__instance.CreateMaterialWrappers(obj.EncapsulateInArray(), ignoreList, objectSupportsUndo);
			}
			List<ISerializable> serializableList = new List<ISerializable>();
			__instance.GetSerializables(serializableList, ignoreList, obj, inspectChildren);

			foreach (ISerializable serializable in serializableList)
			{
				__instance.CreateISerializableInspector(serializable, objectSupportsUndo);
			}

			if (!group)
			{
				return false;
			}

			if (group.inspectChildren_ == Group.InspectChildrenType.All)
			{
				foreach (Transform transform in obj.transform)
				{
					if (transform.gameObject.activeSelf)
					{
						__instance.CreateComponentInspectorsOnObject(ignoreList, objectSupportsUndo, transform.gameObject);
					}
				}
			}
			else
			{
				if (group.inspectChildren_ != Group.InspectChildrenType.Combined)
				{
					return false;
				}

				__instance.CreateComponentInspectorsForObjects(ignoreList, objectSupportsUndo, obj.GetChildren());
			}

			return false;
		}
	}
}
