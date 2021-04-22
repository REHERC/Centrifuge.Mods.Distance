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

		public ExportProjectTask(FileInfo destination, IEditor<CampaignFile> editor)
		{
			this.destination = destination;
			this.editor = editor;
		}

		public override bool Execute(IProgressData progress)
		{
			progress.Status = "Test";


			Dictionary<string, string> hashes = new Dictionary<string, string>();

			using (HashAlgorithm ha = SHA512.Create())
			{
				void hash(string resource)
				{
					progress.Status = $"Calculating hash for {resource}";

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

			return true; // JUST SO THE COMPILER GETS LESS ANNOYING
		}
	}
}
