#pragma warning disable IDE0063
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.UI;
using Newtonsoft.Json;
using System;
using System.IO;

namespace App.AdventureMaker.Core
{
	public static class Project
	{
		public static CampaignFile CreateProject(ProjectCreateData data)
		{
			try
			{
				DirectoryInfo projectDir = Directory.CreateDirectory(data.path);
				DirectoryInfo resourcesDir = projectDir.CreateSubdirectory("resources");

				resourcesDir.CreateSubdirectory("levels");
				resourcesDir.CreateSubdirectory("textures");
				//resourcesDir.CreateSubdirectory("audio"); UNUSED FOR NOW

				CampaignFile project = data.ToProject();
				project.Metadata.Guid = Guid.NewGuid().ToString();

				Json.Save(Path.Combine(projectDir.FullName, "project.json"), project, true);

				return project;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static bool IsValidProjectManifest(string content, out Exception exception)
		{
			exception = null;
			try
			{
				return JsonConvert.DeserializeObject<CampaignFile>(content) != null;
			}
			catch (Exception ex)
			{
				exception = ex;
				return false;
			}
		}

		public static bool IsValidProjectManifest(Stream stream, out Exception exception)
		{
			using (TextReader reader = new StreamReader(stream))
			{
				return IsValidProjectManifest(reader.ReadToEnd(), out exception);
			}
		}

		public static bool IsValidProjectManifest(FileInfo file, out Exception exception)
		{
			using (FileStream stream = File.OpenRead(file.FullName))
			{
				return IsValidProjectManifest(stream, out exception);
			}
		}
	}
}
