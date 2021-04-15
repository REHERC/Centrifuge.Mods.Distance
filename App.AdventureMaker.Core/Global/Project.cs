using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.UI;
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
				resourcesDir.CreateSubdirectory("audio");

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

		public static void ExportProject(FileInfo destination, IEditor<CampaignFile> editor)
		{

		}
	}
}
