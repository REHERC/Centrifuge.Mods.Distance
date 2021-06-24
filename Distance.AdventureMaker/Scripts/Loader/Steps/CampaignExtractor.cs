using Centrifuge.Distance.Game;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Distance.AdventureMaker.Loader.CampaignLoaderLogic;

namespace Distance.AdventureMaker.Loader.Steps
{
	public class CampaignExtractor : LoaderTask
	{
		public CampaignExtractor(CampaignLoader loader) : base(loader)
		{
		}

		public override IEnumerator Run(Task.Status status)
		{
			string id;
			CampaignListing.CampaignItem data;
			FileInfo archivePath;

			foreach (var item in loader.Listing.Archives)
			{
				Mod.Instance.Logger.Info($"[{item.Value.source}]\t {item.Key} : {item.Value.path.FullName}");

				id = item.Key;
				data = item.Value;
				archivePath = data.path as FileInfo;

				DirectoryInfo extractTo;

				if (loader.Listing.Folders.ContainsKey(id))
				{
					extractTo = loader.Listing.Folders[id].path as DirectoryInfo;
				}
				else
				{
					string campaignDirectory = SanitizeFileName(id, Path.GetInvalidFileNameChars);
					extractTo = new DirectoryInfo(Path.Combine(archivePath.Directory.FullName, campaignDirectory));
				}

				using (IArchive archive = ZipArchive.Open(archivePath))
				{
					Dictionary<string, IArchiveEntry> entries = archive
					.GetFileEntries()
					.ToList()
					.Where(entry =>
					{
						if (string.Equals("project.json", entry.Key, StringComparison.InvariantCultureIgnoreCase))
						{
							return true;
						}
						else if (entry.Key.StartsWith("resources/", StringComparison.InvariantCultureIgnoreCase))
						{
							return true;
						}
						return false;
					})
					.ToDictionary(entry => entry.Key, entry => entry.Value);

					FileInfo destination;
					foreach (var entry in entries)
					{
						string entryPath = SanitizeFileName(entry.Key, Path.GetInvalidPathChars);
						destination = new FileInfo(Path.Combine(extractTo.FullName, entryPath));

						Mod.Instance.Logger.Warning(destination.FullName);

						destination.Directory.CreateIfDoesntExist();

						using (Stream fileStream = File.Open(destination.FullName, FileMode.Create))
						{
							status.SetText($"Extracting {item.Value.path.FullName} ...\n{entry.Key}");

							entry.Value.WriteTo(fileStream);
							fileStream.Flush();

							yield return null;
						}
					}
				}
			}

			yield break;
		}

		protected string SanitizeFileName(string id, Func<char[]> invalidCharsCallback)
		{
			char[] invalidChars = invalidCharsCallback?.Invoke() ?? new char[0];

			StringBuilder sb = new StringBuilder(id.Length);

			foreach (char c in id)
			{
				if (!invalidChars.Contains(c))
				{
					sb.Append(c);
				}
			}

			return sb.ToString();
		}
	}
}
