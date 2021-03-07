#pragma warning disable IDE1006
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tools.External.Attributes;
using Tools.External.Forms;

namespace Tools.External.Tools
{
	[Tool("Read decompiled shader properties")]
	public class ReadShaderProperties : Tool
	{
		public static readonly string[] allowed_types = new string[]
		{
			"Color",
			"2D",
			"Float",
			"Cube",
			"Vector",
			"any"
		};

		private string nl => Environment.NewLine;

		private string[] files;
		private LoggerWindow log;
		private LoggerWindow output;
		private List<string> properties;
		private Dictionary<string, List<string>> sorted;
		private int count;

		public override bool Prepare(StateProvider state)
		{
			OpenFileDialog dlg = new OpenFileDialog
			{
				Title = "Select shader files",
				Filter = "Decompiled shaders (.shader)|*.shader",
				SupportMultiDottedExtensions = true,
				AutoUpgradeEnabled = true,
				CheckFileExists = true,
				CheckPathExists = true,
				Multiselect = true
			};

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				files = dlg.FileNames;
				return true;
			}
			else
			{
				return false;
			}
		}

		public override bool Run(StateProvider state)
		{
			log = OpenWindow<LoggerWindow>().Rename("Info");
			properties = new List<string>();
			sorted = new Dictionary<string, List<string>>();
			count = 0;

			state.SetProgress(0);
			state.SetMaxProgress(files.Length);
			foreach (var file in from f in files where File.Exists(f) select new FileInfo(f))
			{
				ReadShader(file);

				state.AddProgress(1);
				state.SetStatus($"[{state.Progress}%] - Reading \"{file.Name}\"...");
			}

			state.SetMaxProgress(properties.Count);
			state.SetProgress(0);
			foreach (var prop in properties)
			{
				AddProperty(prop);
				state.AddProgress(1);
				state.SetStatus($"[{state.Progress}%] - Scanning...");
			}



			output = OpenWindow<LoggerWindow>().Rename("Output");

			state.SetMaxProgress(count);
			state.SetProgress(0);
			foreach (var group in sorted)
			{
				output.WriteLine(string.Format($"=== {group.Key} ==={nl}"));

				foreach (var prop in group.Value.OrderBy(s => s))
				{
					output.WriteLine(prop);
					state.AddProgress(1);
					state.SetStatus($"[{state.Progress}%] - Displaying...");
				}

				output.WriteLine($"{nl}{nl}");
			}

			state.SetStatus("Done!");

			log.Dispose();

			return true;
		}

		public void AddProperty(string property)
		{
			string name = property[0] is '[' ? property.Split(' ')[1] : property.Split(' ')[0];
			string declaration = property.Substring(name.Length + 1).Split('=')[0];

			declaration = declaration.Remove(declaration.Length - 2);

			int prop_type_start = declaration.LastIndexOf(',') + 2;

			string type = declaration.Substring(prop_type_start);

			AddPropertyToCategory(type, name);
		}

		public void AddPropertyToCategory(string category, string property)
		{
			if (!allowed_types.Contains(category))
			{
				return;
			}

			if (!sorted.ContainsKey(category))
			{
				sorted.Add(category, new List<string>());
			}

			if (!sorted[category].Contains(property))
			{
				sorted[category].Add(property);
				count++;
			}
		}

		public void ReadShader(FileInfo file)
		{
			string buffer = File.ReadAllText(file.FullName);

			int start = FindOffset(buffer, "Properties {");

			List<string> props = new List<string>();

			foreach (var line in buffer.Substring(start).Split('\n'))
			{
				if (line == "}")
				{
					break;
				}
				else if (line.Length > 0)
				{
					props.Add(line);
				}
			}

			properties.AddRange(props);

			log.WriteLine($"Found {props.Count} properties in {file.Name}");
		}

		public int FindOffset(string source, string search)
		{
			int start = source.IndexOf(search);
			return start == -1 ? -1 : start + search.Length;
		}
	}
}