#pragma warning disable RCS1110
using UnityEngine;

public static class GameObjectExtensions
{
	public static string FullName(this GameObject obj)
	{
		if (!obj.transform.parent)
		{
			return obj.name;
		}

		return $"{obj.transform.parent.gameObject.FullName()}/{obj.name}";
	}
}