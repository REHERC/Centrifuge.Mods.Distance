using System;
using System.Collections.Generic;
using System.Reflection;
using Tools.External.Attributes;
using Tools.External.Extensions;

namespace Tools.External
{
	public class ToolManager
	{
		public List<Tool> Tools { get; private set; }

		public ToolManager()
		{
			Tools = new List<Tool>();
		}

		public void LoadTools()
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				LoadTools(assembly);
			}
		}

		private void LoadTools(Assembly assembly)
		{
			foreach (Type type in assembly.GetExportedTypes())
			{
				LoadTools(type);
			}
		}

		private void LoadTools(Type type)
		{
			if (type.IsSubclassOf(typeof(Tool)) && !type.IsAbstract && !type.IsInterface && !type.IsGenericType)
			{
				if (type.GetAttribute(out ToolAttribute _))
				{
					Tool instance = Activator.CreateInstance(type) as Tool;
					Tools.Add(instance);
				}
			}
		}

		public string GetToolName(Tool instance)
		{
			Type type = instance.GetType();

			if (type.GetAttribute(out ToolAttribute attribute))
			{
				return attribute.Name;
			}

			return string.Empty;
		}
	}
}
