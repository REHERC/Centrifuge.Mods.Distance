#pragma warning disable IDE0063
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using Distance.AdventureMaker.Common.Models.UI;
using Newtonsoft.Json;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

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
			// Just testing...
			/*using (Stream file = File.Create(destination.FullName))
			{
				using (IWriter archive = WriterFactory.Open(file, ArchiveType.Zip, CompressionType.LZMA))
				{
					archive.CreateEntry("project.json", JsonConvert.SerializeObject(editor.Document));
				}
			}*/

			Dictionary<string, string> hashes = new Dictionary<string, string>();

			using (HashAlgorithm ha = SHA512.Create())
			{
				void hash(string resource)
				{
					FileInfo file = editor.GetResourceFile(resource);
					if (hashes.ContainsKey(resource) || !file.Exists) return;

					using (Stream stream = File.OpenRead(file.FullName))
					{
						byte[] hashed = ha.ComputeHash(stream);
						hashes[resource] = BitConverter.ToString(hashed).Replace("-", "");
					}
				}

				foreach (CampaignResource resource in editor.Document.Data.Resources)
				{
					switch (resource)
					{
						case CampaignResource.Texture texture:
							hash(texture.file);
							break;
						case CampaignResource.Level level:
							hash(level.file);
							hash(level.thumbnail);
							break;
					}
				}
			}

			using (Stream file = File.Create(destination.FullName))
			{
				using (ZipArchive archive = ZipArchive.Create())
				{
					archive.AddEntry("readme.txt", Resources.GetText("archive_readme.txt").GetStream());
					archive.AddEntry("project.json", JsonConvert.SerializeObject(editor.Document).GetStream());
					archive.AddEntry("hashes.json", JsonConvert.SerializeObject(hashes).GetStream());

					void addfile(string resource)
					{
						FileInfo file = editor.GetResourceFile(resource);
						if (!file.Exists) return;

						archive.AddEntry($"resources/{resource}", File.OpenRead(file.FullName));
					}

					foreach (CampaignResource resource in editor.Document.Data.Resources)
					{
						switch (resource)
						{
							case CampaignResource.Texture texture:
								addfile(texture.file);
								break;
							case CampaignResource.Level level:
								addfile(level.file);
								addfile(level.thumbnail);
								break;
						}
					}

					archive.SaveTo(file);
				}
			}
		}
	}
}
