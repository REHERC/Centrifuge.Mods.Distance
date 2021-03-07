#pragma warning disable RCS1110
using System.Reflection;

public static class ObjectExtensions
{
    /// <summary>
    /// Clones a object via shallow copy
    /// </summary>
    /// <typeparam name="T">Object Type to Clone</typeparam>
    /// <param name="obj">Object to Clone</param>
    /// <returns>New Object reference</returns>
    public static T CloneObject<T>(this T obj) where T : class
    {
        if (obj == null)
        {
            return null;
        }

        MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

        if (inst != null)
        {
            return (T)inst.Invoke(obj, null);
        }
        else
        {
            return null;
        }
    }
}