using Centrifuge.Distance.Game;
using Distance.AdventureMaker.Common.Models;
using Newtonsoft.Json;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using static Distance.AdventureMaker.Loader.CampaignLoaderLogic;

namespace Distance.AdventureMaker.Loader.Steps
{
	public class CampaignListing : LoaderTask //, IEnumerable<KeyValuePair<string, CampaignListing.CampaignItem>>
	{
		public readonly CampaignItem.Collection Folders;
		public readonly CampaignItem.Collection Archives;
		public readonly CampaignItem.Collection AllManifests;

		public CampaignListing(CampaignLoader loader) : base(loader)
		{
			Folders = new CampaignItem.Collection();
			Archives = new CampaignItem.Collection();
			AllManifests = new CampaignItem.Collection();
		}

		public override IEnumerator Run(Task.Status status)
		{
			/*
				List all .crab files and extracted folders
				For each folder, check project.json and fill the map
				Proceed to scan every .crab file
				If 
					.crab file cid is not in map
					or cid is in map but version is higher
				Then update extraction map with higher version

				For each item in extraction map

				If update was made, set flag to true
				Later: if flag is true: ask if the mod should destroy the source .crab file
			*/

			status.SetText("Scanning workspace...");
			status.SetProgress(0, 1);

			foreach (DirectoryInfo workspace in loader.Workspace)
			{
				// Scan already extracted campaigns first
				foreach (DirectoryInfo directory in workspace.GetDirectories())
				{
					FileInfo projectFile = new FileInfo(Path.Combine(directory.FullName, "project.json"));

					if (projectFile.Exists)
					{
						CampaignFile manifest = Json.Load<CampaignFile>(projectFile, null);

						if (manifest != null)
						{
							Folders.Add(manifest, directory);
							AllManifests.Add(manifest, directory);
						}
					}
				}

				// Scan archive files
				foreach (FileInfo file in workspace.GetFiles("*.crab"))
				{
					using (Stream archiveStream = File.OpenRead(file.FullName))
					{
						if (!ZipArchive.IsZipFile(archiveStream))
						{
							continue;
						}

						using (IArchive archive = ZipArchive.Open(archiveStream))
						{
							Dictionary<string, IArchiveEntry> entries = archive.GetFileEntries();

							bool required_entries_present =
							   entries.ContainsKey("$campaign")
							&& entries.ContainsKey("project.json")
							&& entries.ContainsKey("hashes.json");

							if (!required_entries_present)
							{
								continue;
							}

							Dictionary<string, string> hashes = JsonConvert.DeserializeObject<Dictionary<string, string>>(entries["hashes.json"].GetText());

							string hash(string entry)
							{
								using (HashAlgorithm ha = SHA512.Create())
								{
									byte[] hashed = ha.ComputeHash(entries[$"resources/{entry.Replace("\\", "/")}"].OpenEntryStream());
									return BitConverter.ToString(hashed).Replace("-", "");
								}
							}

							if (!hashes.All(hashEntry => string.Equals(hashEntry.Value, hash(hashEntry.Key))))
							{
								continue;
							}

							try
							{
								CampaignFile manifest = JsonConvert.DeserializeObject<CampaignFile>(entries["project.json"].GetText());

								if (manifest != null)
								{
									Archives.Add(manifest, file);
									AllManifests.Add(manifest, file);
								}
							}
							catch (Exception)
							{
								continue;
							}
						}
					}
				}
			}

			//status.SetText("");
			//status.SetProgress(0, 1);

			//yield return Task.Wait(5.5f);
			yield break;
		}

		/*public IEnumerator<KeyValuePair<string, CampaignItem>> GetEnumerator()
		{
			return AllManifests.GetEnumerator();
		}*/

		public struct CampaignItem : IEquatable<CampaignItem>, IComparable<CampaignItem>
		{
			public enum Source : byte
			{
				Archive = 0,
				Folder = 1
			}

			public class Collection : Dictionary<string, CampaignItem>
			{
				public void Add(CampaignFile manifest, DirectoryInfo directory)
				{
					Add(manifest, directory, Source.Folder);
				}

				public void Add(CampaignFile manifest, FileInfo file)
				{
					Add(manifest, file, Source.Archive);
				}

				public void Add(CampaignFile manifest, FileSystemInfo fsi, Source source)
				{
					CampaignMetadata metadata = manifest.Metadata;
					CampaignItem item = new CampaignItem(source, fsi, metadata.Version);

					if (!ContainsKey(metadata.Guid))
					{
						this[metadata.Guid] = item;
					}
					else
					{
						if (item > this[metadata.Guid])
						{
							this[metadata.Guid] = item;
						}
					}
				}
			}

			public Source source;
			public FileSystemInfo path;
			public long version;

			public CampaignItem(Source source, FileSystemInfo path, long version)
			{
				this.source = source;
				this.path = path;
				this.version = version;
			}

			public int CompareTo(CampaignItem other)
			{
				int versionCompare = version.CompareTo(other.version);
				if (versionCompare == 0)
				{
					return source.CompareTo(other.source);
				}
				return versionCompare;
			}

			public override bool Equals(object obj)
			{
				return obj is CampaignItem info && Equals(info);
			}

			public bool Equals(CampaignItem other)
			{
				return version == other.version && source == other.source;
			}

			public override int GetHashCode()
			{
				int hashCode = -1235800969;
				hashCode = (hashCode * -1521134295) + version.GetHashCode();
				hashCode = (hashCode * -1521134295) + source.GetHashCode();
				return hashCode;
			}

			public static bool operator ==(CampaignItem left, CampaignItem right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(CampaignItem left, CampaignItem right)
			{
				return !(left == right);
			}

			public static bool operator >(CampaignItem left, CampaignItem right)
			{
				return left.CompareTo(right) == 1;
			}

			public static bool operator <(CampaignItem left, CampaignItem right)
			{
				return right.CompareTo(left) == 1;
			}

			public static bool operator >=(CampaignItem left, CampaignItem right)
			{
				return left > right || left == right;
			}

			public static bool operator <=(CampaignItem left, CampaignItem right)
			{
				return left < right || left == right;
			}
		}
	}
}
