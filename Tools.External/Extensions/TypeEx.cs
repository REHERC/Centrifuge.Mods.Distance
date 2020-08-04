using System;

namespace Tools.External.Extensions
{
    public static class TypeEx
    {
        /// <summary>
        /// Returns if an attribute of type <typeparamref name="T"/> exists and passes it as an output parameter
        /// </summary>
        public static bool GetAttribute<T>(this Type type, out T attribute, bool inherit = true) where T : Attribute
        {
            attribute = null;

            foreach (var attr in type.GetCustomAttributes(inherit))
            {
                if (attr is T)
                {
                    attribute = attr as T;
                    return true;
                }
            }

            return false;
        }
    }
}
