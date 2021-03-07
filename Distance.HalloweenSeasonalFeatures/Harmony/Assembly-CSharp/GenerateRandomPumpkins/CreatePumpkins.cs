using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.HalloweenSeasonalFeatures.Harmony
{
	[HarmonyPatch(typeof(GenerateRandomPumpkins), "CreatePumpkins")]
	internal static class GenerateRandomPumpkins__CreatePumpkins
	{
		[HarmonyPrefix]
		internal static bool Prefix(GenerateRandomPumpkins __instance)
		{
			/*__instance.pumpkinPrefabs_ = new GameObject[]
			{
				Resources.Load<GameObject>("Prefabs/LevelEditor/Decorations/EmpireLamp"),
				Resources.Load<GameObject>("Prefabs/LevelEditor/Decorations/EmpireLamp02"),
				Resources.Load<GameObject>("Prefabs/LevelEditor/Decorations/Echoes/NitronicLamp")
			};*/

			List<TrackSegment> componentsOfType = G.Sys.GameManager_.Level_.FindComponentsOfType<TrackSegment>();

			__instance.maxCount_ = componentsOfType.Count;
			//__instance.maxCount_ = componentsOfType.Count * 100;
			//__instance.segmentFrequency_ = 7.8f;
			//__instance.percentageOfRoadWidth_ = 0.99f;
			//__instance.maxPumpkinsPerSegment_ = 85;

			RandomEx.seed = G.Sys.GameManager_.LevelName_.GetHashCode();

			List<GenerateRandomPumpkins.PumpkinInfos> pums = new List<GenerateRandomPumpkins.PumpkinInfos>();

			IterateOverSegments(componentsOfType, pums, __instance);

			foreach (GenerateRandomPumpkins.PumpkinInfos pumpkinInfos in pums)
			{
				GameObject gameObject = Resource.LoadPrefabInstance(pumpkinInfos.name);
				//GameObject gameObject = __instance.pumpkinPrefabs_.RandomElement();
				gameObject.transform.position = pumpkinInfos.pos;
				gameObject.transform.rotation = pumpkinInfos.rot;
				gameObject.GetComponent<OnCollisionBreakApartLogic>().ignoreEvents_ = true;
				//gameObject.AddComponent<HolidayFeaturesToggle>();
			}

			return false;
		}

		internal static void IterateOverSegments(List<TrackSegment> segments, List<GenerateRandomPumpkins.PumpkinInfos> pums, GenerateRandomPumpkins __instance)
		{
			for (int index = 0; index <= __instance.maxPumpkinsPerSegment_; ++index)
			{
				foreach (TrackSegment segment in segments)
				{
					if (segment.DagEdgeCount_ != 0 && Random.Range(0.0f, 1f) <= __instance.segmentFrequency_)
					{
						segment.CalcTrackInfo(out Track.PointInfo info, Random.Range(0.05f, 0.95f));

						if (Vector3.Angle(info.Up_, Vector3.up) <= 35.0)
						{
							float max = info.scale_ * __instance.percentageOfRoadWidth_;
							Vector3 _pos = info.posOnTrack_ + (Random.Range(-max, max) * info.Right_);
							Quaternion _rot = Quaternion.LookRotation(info.Forward_, info.Up_) * Quaternion.AngleAxis(Random.Range(0.0f, 359f), info.Up_);

							string name = __instance.pumpkinPrefabs_[Random.Range(0, __instance.pumpkinPrefabs_.Length)].name;
							pums.Add(new GenerateRandomPumpkins.PumpkinInfos(name, _pos, _rot));

							--__instance.maxCount_;

							if (__instance.maxCount_ == 0)
							{
								return;
							}
						}
					}
				}
			}
		}
	}
}
