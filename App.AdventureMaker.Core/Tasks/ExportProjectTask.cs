#pragma warning disable IDE0063
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using Newtonsoft.Json;
using SharpCompress.Archives.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace App.AdventureMaker.Core.Tasks
{
	public class ExportProjectTask : TaskBase
	{
		private readonly FileInfo destination;
		private readonly IEditor<CampaignFile> editor;

		public ExportProjectTask(IEditor<CampaignFile> editor, FileInfo destination)
		{
			this.destination = destination;
			this.editor = editor;
		}

		public override async Task Execute(Progress progress)
		{
			progress.Title = "Exporting project...";
			progress.Maximum = editor.Document.Data.Resources.Count;

			Dictionary<string, string> hashes = new Dictionary<string, string>();

			using (HashAlgorithm ha = SHA512.Create())
			{
				void hash(string resource)
				{
					string key = resource.Replace("\\", "/");

					progress.Status = $"Calculating hash for \"{Path.GetFileName(resource)}\"";

					FileInfo file = editor.GetResourceFile(resource);
					if (hashes.ContainsKey(key) || !file.Exists) return;

					using (Stream stream = File.OpenRead(file.FullName))
					{
						byte[] hashed = ha.ComputeHash(stream);
						hashes[key] = BitConverter.ToString(hashed).Replace("-", "");
					}
				}

				foreach (CampaignResource resource in editor.Document.Data.Resources)
				{
					progress.Status = $"Calculating dependencies hash for resource \"{resource.guid}\"";
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

					progress.Value++;
				}
			}

			progress.Value = 0;
			progress.Maximum = hashes.Count + 4;

			using (Stream file = File.Create(destination.FullName))
			{
				using (ZipArchive archive = ZipArchive.Create())
				{
					progress.Status = "Packaging manifests...";
					archive.AddEntry("readme.txt", Resources.GetText("archive_readme.txt").GetStream());
					progress.Value++;
					archive.AddEntry("$campaign", string.Empty.GetStream());
					progress.Value++; 
					editor.Document.Metadata.Version = DateTime.Now.TimeOfDay.Ticks;
					archive.AddEntry("project.json", JsonConvert.SerializeObject(editor.Document).GetStream());
					progress.Value++;
					archive.AddEntry("hashes.json", JsonConvert.SerializeObject(hashes).GetStream());
					progress.Value++;

					void addfile(string resource)
					{
						progress.Status = $"Packaging file \"{Path.GetFileName(resource)}\"...";

						FileInfo file = editor.GetResourceFile(resource);
						if (!file.Exists) return;

						archive.AddEntry($"resources/{resource}", File.OpenRead(file.FullName));
						progress.Value++;
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
