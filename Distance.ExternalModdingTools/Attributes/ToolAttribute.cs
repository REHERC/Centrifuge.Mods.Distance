using System;

namespace Distance.ExternalModdingTools.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ToolAttribute : Attribute
    {
        public string Name { get; set; }

        public ToolAttribute(string name)
        {
            Name = name;
        }
    }
}
