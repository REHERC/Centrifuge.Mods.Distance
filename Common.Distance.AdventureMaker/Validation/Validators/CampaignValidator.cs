using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Distance.AdventureMaker.Common.Validation.Validators
{
	public class CampaignValidator : Validator<CampaignFile>
	{
		#region Members / Properties and Constructor
		public DirectoryInfo Directory { get; }

		public CampaignValidator(DirectoryInfo dir)
		{
			Directory = dir;
		}

		public FileInfo GetFile(string relativePath)
		{
			return new FileInfo(Path.Combine(Directory.FullName, $"resources/{relativePath}"));
		}
		#endregion

		// A giant validation function... perhaps too big...
		// TODO: Subdivide this piece of code... one day... i'll do that tommorow (:
		public override void Validate(CampaignFile file)
		{
			#region Metadata validity checks
			Log(StatusLevel.Info, "Validating campaign metadata...");

			CampaignMetadata metadata = file.Metadata;
			if (string.IsNullOrEmpty(metadata.Guid))
				Log(StatusLevel.Error, "The campaign unique ID cannot be null and must be renewed");

			if (string.IsNullOrEmpty(metadata.Title))
				Log(StatusLevel.Error, "The campaign title cannot be empty");

			if (string.IsNullOrEmpty(metadata.Description))
				Log(StatusLevel.Warning, "The campaign description is empty");

			if (string.IsNullOrEmpty(metadata.Author))
				Log(StatusLevel.Warning, "The campaign author has not been specified");
			#endregion

			#region Resources validity check
			Log(StatusLevel.Info, "Validating campaign resource files...");
			foreach (CampaignResource resource in file.Data.Resources)
			{
				switch (resource)
				{
					case CampaignResource.Texture texture:
						if (string.IsNullOrEmpty(texture.file))
							Log(StatusLevel.Error, "The file path for the texture cannot be empty");

						FileInfo textureFile = GetFile(texture.file);
						if (!textureFile.Exists)
							Log(StatusLevel.Error, $"The following file could not be found: \"{textureFile.FullName}\"");

						break;
					case CampaignResource.Level level:
						if (string.IsNullOrEmpty(level.file))
							Log(StatusLevel.Error, "The file path for the level file cannot be empty");

						FileInfo levelFile = GetFile(level.file);
						if (!levelFile.Exists)
							Log(StatusLevel.Error, $"The following file could not be found: \"{levelFile.FullName}\"");

						if (string.IsNullOrEmpty(level.thumbnail))
							Log(StatusLevel.Error, "The file path for the level thumbnail file cannot be empty");

						FileInfo thumbnailFile = GetFile(level.thumbnail);
						if (!thumbnailFile.Exists)
							Log(StatusLevel.Error, $"The following file could not be found: \"{levelFile.FullName}\"");

						break;
				}
			}
			#endregion

			// For easy .Contains calls
			HashSet<string> resources = new HashSet<string>(file.Data.Resources.Select(resource => resource.guid));

			#region Playlist checks
			Log(StatusLevel.Info, "Validating campaign level playlists...");

			List<CampaignPlaylist> playlists = file.Data.Playlists;

			if (playlists.Count == 0)
			{
				Log(StatusLevel.Error, "No playlist declared");
			}
			else
			{
				if (!playlists.Any(playlist => playlist.DisplayInCampaign))
					Log(StatusLevel.Error, "The campaign must have at least one campaign playlist");

				foreach (CampaignPlaylist playlist in playlists)
				{
					Log(StatusLevel.Info, $"Validating playlist #{playlists.IndexOf(playlist) + 1} ({playlist.Name})");

					if (string.IsNullOrEmpty(playlist.Guid))
						Log(StatusLevel.Error, "The playlist unique ID cannot be null and must be renewed");

					if (string.IsNullOrEmpty(playlist.Name))
						Log(StatusLevel.Error, "The playlist name cannot be empty");

					if (string.IsNullOrEmpty(playlist.Description))
						Log(StatusLevel.Warning, $"The playlist description for \"{playlist.Name}\" is empty");

					if (!playlist.DisplayInCampaign && !playlist.DisplayInSprint)
						Log(StatusLevel.Warning, $"The playlist \"{playlist.Name}\" cannot be displayed as its visibility in sprint and campaigns is set to none");

					#region Level checks
					List<CampaignLevel> levels = playlist.Levels;

					if (levels.Count == 0)
					{
						Log(StatusLevel.Error, $"The playlist \"{playlist.Name}\" contains no levels");
					}
					else
					{
						Log(StatusLevel.Info, $"Validating playlist levels for \"{playlist.Name}\"...");
						foreach (CampaignLevel level in levels)
						{
							Log(StatusLevel.Info, $"Validating level {levels.IndexOf(level) + 1} ({level.Name})...");

							if (string.IsNullOrEmpty(level.Guid))
								Log(StatusLevel.Error, "The level unique ID cannot be null and must be renewed");

							if (string.IsNullOrEmpty(level.Name))
								Log(StatusLevel.Error, $"The level name is not set");

							if (string.IsNullOrEmpty(level.ResourceId))
								Log(StatusLevel.Error, $"The level file reference for \"{level.Name}\" is not set");

							if (!resources.Contains(level.ResourceId))
								Log(StatusLevel.Error, $"The level file reference for \"{level.Name}\" is invalid");

							if (!string.IsNullOrEmpty(level.LoadingBackground) && !resources.Contains(level.LoadingBackground))
								Log(StatusLevel.Error, $"The file reference for the loading background image of \"{level.Name}\" is invalid");

							if (!string.IsNullOrEmpty(level.LoadingBackgroundIcon) && !resources.Contains(level.LoadingBackgroundIcon))
								Log(StatusLevel.Error, $"The file reference for the loading icon of \"{level.Name}\" is invalid");
						}
					}
					#endregion
				}
			}
			#endregion

			#region Old check code
			//if (metadata is null)
			//{
			//	Log(StatusLevel.Error, "The metadata section was null when validating (metadata)");
			//}
			//else
			//{
			//	if (string.IsNullOrEmpty(metadata.Guid))
			//	{
			//		Log(StatusLevel.Error, "The campaign identifier can't be null (metadata.guid)");
			//	}

			//	if (string.IsNullOrEmpty(metadata.Title))
			//	{
			//		Log(StatusLevel.Error, "The campaign title can't be null (metadata.title)");
			//	}

			//	if (string.IsNullOrEmpty(metadata.Description))
			//	{
			//		Log(StatusLevel.Warning, "The campaign description is null (metadata.description)");
			//	}
			//}

			//CampaignData data = file.Data;

			//if (data is null)
			//{
			//	Log(StatusLevel.Error, "The data section was null when validating (data)");
			//}
			//else
			//{
			//	List<CampaignPlaylist> playlists = data.Playlists;
			//	List<CampaignResource> resources = data.Resources;

			//	if (playlists is null)
			//	{
			//		Log(StatusLevel.Error, "The playlists section was null when validating (data.playlists[])");
			//	}
			//	else if (playlists.Count == 0)
			//	{
			//		Log(StatusLevel.Error, "The campaign doesn't have any playlist (data.playlists[])");
			//	}
			//	else
			//	{
			//		foreach (CampaignPlaylist playlist in playlists)
			//		{
			//			if (playlist is null)
			//			{
			//				Log(StatusLevel.Warning, "The playlist item was null when validating (data.playlists[])");
			//			}
			//			else
			//			{
			//				if (string.IsNullOrEmpty(playlist.Guid))
			//				{
			//					Log(StatusLevel.Error, "The playlist identifier can't be null (item.playlists[].guid)");
			//				}

			//				if (string.IsNullOrEmpty(playlist.Name))
			//				{
			//					Log(StatusLevel.Error, "The playlist name can't be null (item.playlists[].name)");
			//				}

			//				List<CampaignLevel> levels = playlist.Levels;

			//				if (levels is null)
			//				{
			//					Log(StatusLevel.Error, $"The levels section was null when validating the playlist \"{playlist.Name}\" (item.playlists[].levels[]) !");
			//				}
			//				else if (levels.Count == 0)
			//				{
			//					Log(StatusLevel.Warning, $"The \"{playlist.Name}\" playlist doesn't contain any level");
			//				}
			//				else
			//				{
			//					foreach (CampaignLevel level in levels)
			//					{
			//						if (level is null)
			//						{
			//							Log(StatusLevel.Error, "The level playlist item was null when validating (data.playlists[].levels[])");
			//						}
			//						else
			//						{
			//							if (string.IsNullOrEmpty(level.ResourceId))
			//							{
			//								Log(StatusLevel.Error, "The level playlist item was null when validating (data.playlists[].levels[])");
			//							}
			//							else
			//							{
			//								if (string.IsNullOrEmpty(level.ResourceId))
			//								{
			//									Log(StatusLevel.Error, "The associated level file is invalid (data.playlists[].levels[].resource_id)");
			//								}
			//								else
			//								{
			//									CampaignResource[] resources_search = resources.Where(res => Equals(res.guid, level.ResourceId) && res.resource_type == ResourceType.Level).ToArray();

			//									if (resources_search.Length != 1)
			//									{
			//										Log(StatusLevel.Error, "The level playlist item was null when validating (data.playlists[].levels[])");
			//									}
			//								}
			//							}
			//						}
			//					}
			//				}
			//			}
			//		}
			//	}
			//}
			#endregion
		}
	}
}
