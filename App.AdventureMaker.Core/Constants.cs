using App.AdventureMaker.Core.Forms.Dialog;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models.Resources;
using System;
using System.Collections.Generic;

namespace App.AdventureMaker.Core
{
	public static class Constants
	{
		public const string GITHUB_ISSUES_PAGE = "https://github.com/REHERC/Centrifuge.Mods.Distance/issues/new";
		public const string GITHUB_WIKI = "https://github.com/REHERC/Centrifuge.Mods.Distance/wiki";
		public const string DISCORD_DISTANCE = "https://discord.gg/distance";

		public const string DIALOG_FILTER_PROJECT = "Json project files (project.json)|project.json";
		public const string DIALOG_FILTER_ANY = "All files (*.*)|*.*";

		public const string DIALOG_MESSAGE_EDITOR_PREVIEW = "This software is currently still work in progress!\nPlease only use it for testing purposes and feedback as many breaking changes may occur in the future.";
		public const string DIALOG_MESSAGE_UNSAVED_CHANGES = "The currently opened file has unsaved changes!\nContinue without saving?";
		public const string DIALOG_MESSAGE_REMOVE_PLAYLIST = "Are you sure you want to remove the following playlist: \"{0}\" ?";
		public const string DIALOG_MESSAGE_REMOVE_RESOURCE = "Are you sure you want to remove this resource ?\nObjects requiring this resource will need to be updated manually !";

		public const string DIALOG_CAPTION_EDITOR_PREVIEW = "Campaign Editor - Preview";
		public const string DIALOG_CAPTION_APP_CLOSE = "Close application";
		public const string DIALOG_CAPTION_NEW_FILE = "New file";
		public const string DIALOG_CAPTION_CLOSE_PROJECT = "Close project";
		public const string DIALOG_CAPTION_REMOVE_PLAYLIST = "Remove playlist";
		public const string DIALOG_CAPTION_REMOVE_RESOURCE = "Remove resource";

		public const string PLAYLIST_NO_NAME = "(No playlist name)";

		public static readonly Dictionary<ResourceType, Func<CampaignResource, CampaignResource>> RESOURCE_DIALOGS = new Dictionary<ResourceType, Func<CampaignResource, CampaignResource>>()
		{
			{ ResourceType.Level, (data) => new LevelDialog(data as CampaignResource.Level).ShowModal() },
			{ ResourceType.Texture, (data) => new TextureDialog(data as CampaignResource.Texture).ShowModal() }
		};
	}
}
