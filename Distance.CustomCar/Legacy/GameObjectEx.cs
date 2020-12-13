using UnityEngine;

namespace CustomCar.Legacy
{
    public static class GameObjectEx
    {
        public static string FullName(this GameObject obj)
        {
            if (obj.transform.parent == null)
            {
                return obj.name;
            }

            return obj.transform.parent.gameObject.FullName() + "/" + obj.name;
        }
    }
}