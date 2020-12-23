using Reactor.API.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Distance.SceneDumper
{
	public class Dumper
	{
		private FileSystem FileSystem { get; }

		internal Dumper(FileSystem fileSystem)
		{
			FileSystem = fileSystem;
		}

		public void DumpCurrentScene(bool detailed)
		{
			var currentSceneName = SceneManager.GetActiveScene().name;
			var dateTimeIdentifier = DateTime.Now.ToString("ddMMyyyy_HHmmss");
			var gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

			var validGameObjects = new List<GameObject>();
			foreach (var gameObject in gameObjects)
			{
				if (gameObject.transform.parent == null)
				{
					validGameObjects.Add(gameObject);
				}
			}

			using (var sw = new StreamWriter(FileSystem.CreateFile($"{currentSceneName}.{dateTimeIdentifier}.txt")))
			{
				foreach (var gameObject in validGameObjects)
				{
					DumpGameObject(gameObject, sw, "", detailed);
				}
			}
		}

		private void DumpGameObject(GameObject gameObject, StreamWriter writer, string indent, bool detailed)
		{
			writer.WriteLine($"{indent}+{gameObject.name}");

			foreach (var component in gameObject.GetComponents<Component>())
			{
				DumpComponent(component, writer, indent + "  ", detailed);
			}

			foreach (Transform child in gameObject.transform)
			{
				DumpGameObject(child.gameObject, writer, indent + "  ", detailed);
			}
		}

		private void DumpComponent(Component component, StreamWriter writer, string indent, bool detailed)
		{
			writer.WriteLine($"{indent}{component?.GetType().Name ?? "(null)"}");

			if (detailed)
			{
				DumpFields(component, writer, indent + "  ");
			}
		}

		private void DumpFields(Component component, StreamWriter writer, string indent)
		{
			var publicFields = component.GetType().GetFields();
			var nonPublicFields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

			foreach (var field in nonPublicFields)
			{
				writer.WriteLine($"{indent}{field.Name} = {field.GetValue(component)}");
			}

			foreach (var field in publicFields)
			{
				writer.WriteLine($"{indent}{field.Name} = {field.GetValue(component)}");
			}
		}
	}
}
