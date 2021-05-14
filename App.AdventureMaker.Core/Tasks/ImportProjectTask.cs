using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Newtonsoft.Json;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace App.AdventureMaker.Core.Tasks
{
	public class ImportProjectTask : TaskBase
	{
		private readonly IEditor<CampaignFile> editor;
		private readonly FileInfo file;
		private readonly DirectoryInfo directory;

		private bool result;
		private Exception error;

		public ImportProjectTask(IEditor<CampaignFile> editor, FileInfo file)
		: this (editor, file, file?.Directory)
		{}

		public ImportProjectTask(IEditor<CampaignFile> editor, FileInfo file, DirectoryInfo directory)
		{
			this.editor = editor;
			this.file = file;
			this.directory = directory;
			this.result = true;
		}

		public bool GetResult()
		{
			return result;
		}

		public Exception GetError()
		{
			return error;
		}

		private void Fail(Exception error)
		{
			this.error = error;
			result = false;
		}

		public override async Task Execute(Progress progress)
		{
			progress.Title = "Importing project...";
			progress.Status = "Reading archive...";

			if (!ZipArchive.IsZipFile(file))
			{
				Fail(new InvalidDataException("The specified file is not a valid zip archive"));
				return;
			}

			//await Task.Delay(500).ConfigureAwait(false);

			try
			{
				using (Stream stream = File.OpenRead(file.FullName))
				{
					using (IArchive archive = ZipArchive.Open(stream))
					{
						Dictionary<string, IArchiveEntry> entries = archive.GetFileEntries();

						bool required_entries_present =
						   entries.ContainsKey("$campaign")
						&& entries.ContainsKey("project.json")
						&& entries.ContainsKey("hashes.json");

						if (!required_entries_present)
						{
							Fail(new InvalidDataException("The specified file is not a valid zip archive"));
							return;
						}

						Dictionary<string, string> hashes =	JsonConvert.DeserializeObject<Dictionary<string, string>>(entries["hashes.json"].GetText());
						progress.Maximum = hashes.Count;
						progress.Value = 0;

						string hash(string entry)
						{
							using (HashAlgorithm ha = SHA512.Create())
							{
								byte[] hashed = ha.ComputeHash(entries[$"resources/{entry.Replace("\\", "/")}"].OpenEntryStream());
								return BitConverter.ToString(hashed).Replace("-", "");
							}
						}

						foreach (KeyValuePair<string, string> hashEntry in hashes)
						{
							progress.Status = $"Checking data validity... ({progress.Value + 1}/{progress.Maximum})";

							if (!string.Equals(hashEntry.Value, hash(hashEntry.Key)))
							{
								Fail(new InvalidDataException($"File \"{hashEntry.Key}\" hash didn't match the contents of the hash table..."));
								return;
							}

							progress.Value++;
						}

						progress.Maximum = hashes.Count + 1;
						progress.Value = 0;

						foreach (string entryName in hashes.Keys)
						{
							progress.Status = $"Extracting files ... ({progress.Value + 1}/{progress.Maximum})";

							var zipEntry = entries[$"resources/{entryName.Replace("\\", "/")}"];
							Extract(zipEntry);

							progress.Value++;
						}

						Extract(entries["project.json"]);
						progress.Value++;
					}
				}
			}
			catch (Exception error)
			{
				Fail(error);
				return;
			}
		}

		private void Extract(IArchiveEntry entry)
		{
			FileInfo destination = new FileInfo(Path.Combine(directory.FullName, entry.Key));

			destination.Directory.CreateIfDoesntExist();

			using (Stream zipStream = entry.OpenEntryStream())
			{
				using (Stream fileStream = File.Create(destination.FullName))
				{
					zipStream.CopyTo(fileStream);
				}
			}
		}
	}
}
