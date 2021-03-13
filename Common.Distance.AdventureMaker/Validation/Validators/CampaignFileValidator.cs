using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Distance.AdventureMaker.Common.Validation.Validators
{
	public class CampaignFileValidator : Validator<CampaignFile>
	{
		#region Members / Properties and Constructor
		public DirectoryInfo Directory { get; }

		public CampaignFileValidator(DirectoryInfo dir)
		{
			Directory = dir;
		}
		#endregion

		// A giant validation function... perhaps too big...
		// TODO: Subdivide this piece of code... one day... i'll do that tommorow (:
		public override void Validate(CampaignFile file)
		{
			CampaignMetadata metadata = file.Metadata;

			if (metadata is null)
			{
				Log(StatusLevel.ERR, "The metadata section was null when validating (metadata)");
			}
			else
			{
				if (string.IsNullOrEmpty(metadata.Guid))
				{
					Log(StatusLevel.ERR, "The campaign identifier can't be null (metadata.guid)");
				}

				if (string.IsNullOrEmpty(metadata.Title))
				{
					Log(StatusLevel.ERR, "The campaign title can't be null (metadata.title)");
				}

				if (string.IsNullOrEmpty(metadata.Description))
				{
					Log(StatusLevel.WRN, "The campaign description is null (metadata.description)");
				}
			}

			CampaignData data = file.Data;

			if (data is null)
			{
				Log(StatusLevel.ERR, "The data section was null when validating (data)");
			}
			else
			{
				List<CampaignPlaylist> playlists = data.Playlists;
				List<CampaignResource> resources = data.Resources;

				if (playlists is null)
				{
					Log(StatusLevel.ERR, "The playlists section was null when validating (data.playlists[])");
				}
				else if (playlists.Count == 0)
				{
					Log(StatusLevel.ERR, "The campaign doesn't have any playlist (data.playlists[])");
				}
				else
				{
					foreach (CampaignPlaylist playlist in playlists)
					{
						if (playlist is null)
						{
							Log(StatusLevel.WRN, "The playlist item was null when validating (data.playlists[])");
						}
						else
						{
							if (string.IsNullOrEmpty(playlist.Guid))
							{
								Log(StatusLevel.ERR, "The playlist identifier can't be null (item.playlists[].guid)");
							}

							if (string.IsNullOrEmpty(playlist.Name))
							{
								Log(StatusLevel.ERR, "The playlist name can't be null (item.playlists[].name)");
							}

							List<CampaignLevel> levels = playlist.Levels;

							if (levels is null)
							{
								Log(StatusLevel.ERR, $"The levels section was null when validating the playlist \"{playlist.Name}\" (item.playlists[].levels[]) !");
							}
							else if (levels.Count == 0)
							{
								Log(StatusLevel.WRN, $"The \"{playlist.Name}\" playlist doesn't contain any level");
							}
							else
							{
								foreach (CampaignLevel level in levels)
								{
									if (level is null)
									{
										Log(StatusLevel.ERR, "The level playlist item was null when validating (data.playlists[].levels[])");
									}
									else
									{
										if (string.IsNullOrEmpty(level.ResourceId))
										{
											Log(StatusLevel.ERR, "The level playlist item was null when validating (data.playlists[].levels[])");
										}
										else
										{
											if (string.IsNullOrEmpty(level.ResourceId))
											{
												Log(StatusLevel.ERR, "The associated level file is invalid (data.playlists[].levels[].resource_id)");
											}
											else
											{
												CampaignResource[] resources_search = resources.Where(res => Equals(res.guid, level.ResourceId) && res.resource_type == ResourceType.Level).ToArray();

												if (resources_search.Length != 1)
												{
													Log(StatusLevel.ERR, "The level playlist item was null when validating (data.playlists[].levels[])");
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
