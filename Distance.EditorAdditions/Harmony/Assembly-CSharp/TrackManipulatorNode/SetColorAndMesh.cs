using HarmonyLib;
using System;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(TrackManipulatorNode), "SetColorAndMesh", new System.Type[0])]
	internal static class TrackManipulatorNode__SetColorAndMesh
	{
		[HarmonyPrefix]
		internal static bool Prefix(TrackManipulatorNode __instance)
		{
			if (!__instance.IsLinkConnected_)
			{
				return true;
			}
			else
			{
				//GameObject lamp = G.Sys.ResourceManager_.GetLevelPrefab("EmpireLamp");
				//Mesh lampMesh = lamp.GetComponent<MeshFilter>().mesh;

				string connectionA = __instance?.Link_?.Segment_?.name;
				string connectionB = __instance?.Link_?.ConnectedSegment_?.name;

				string[] splineNames = new string[2] { connectionA, connectionB };
				Array.Sort(splineNames);

				string colorKey = string.Equals(connectionA, connectionB)
				? connectionA
				: string.Join(string.Empty, splineNames);

				Color? color = Mod.Instance.TrackNodeColors[colorKey];

				if (color != null)
				{
					__instance.SetColorAndMesh((Color)color, __instance.linkedMesh_);
					return false;
				}
				else
				{
					return true;
				}
			}
		}
	}
}
