using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
	[HarmonyPatch(typeof(Group), "Visit")]
	internal static class Group__Visit
	{
		[HarmonyPrefix]
		internal static bool Prefix(Group __instance, IVisitor visitor)
		{
			if (!(visitor is Serializers.Serializer) && !(visitor is Serializers.Deserializer))
			{
				GameObject[] SubObjects = __instance.gameObject.GetChildren();

				if (SubObjects.Length == 0)
				{
					visitor.VisualLabel("No child objects found!".Colorize(Color.white));
					return false;
				}

				if (SubObjects.Length > 0)
				{
					visitor.VisualLabel("Group Hierarchy");

					int Index = 1;

					foreach (GameObject Children in SubObjects)
					{
						string Name = Children.name;

						if (Children.HasComponent<CustomName>())
						{
							CustomName CustomNameComponent = Children.GetComponent<CustomName>();
							Name = $"[b]{CustomNameComponent.CustomName_}[/b]".Colorize(Color.white);
						}

						visitor.VisitAction($"Inspect {Name} (#{Index})", () => EditorUtil.Inspect(Children), null);

						++Index;
					}
				}
			}

			return true;
		}
	}
}
