using App.AdventureMaker.Core.Forms.ResourceDialogs;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models.Resources;
using System;
using System.Collections.Generic;

public static partial class Constants
{
	public const string DISTANCE_STEAM_PROTOCOL_HANDLER_URL = "steam://rungameid/233610//";

	public const string GITHUB_ISSUES_PAGE = "https://github.com/REHERC/Centrifuge.Mods.Distance/issues";
	public const string GITHUB_WIKI = "https://github.com/REHERC/Centrifuge.Mods.Distance/wiki";
	public const string DISCORD_DISTANCE = "https://discord.gg/distance";

	// File filters
	public const string DIALOG_FILTER_ANY = "All files|";

	// Tool-specific filters
	public const string DIALOG_FILTER_PROJECT = "Json project files (project.json)|project.json";

	// Image filters
	public const string DIALOG_FILTER_PNG = "Portable Network Graphics (*.png)|*.png";
	public const string DIALOG_FILTER_JPG = "Joint Photographic Experts Group (*.jpeg;*.jpg;*.jpe;*.jif;*.jfif;*.jfi)|*.jpeg;*.jpg;*.jpe;*.jif;*.jfif;*.jfi";
	public const string DIALOG_FILTER_TGA = "Truevision Advanced Raster Graphics Adapter (*.tga)|*.tga";
	public const string DIALOG_FILTER_TIF = "Tag Image File Format (*.tif;*.tiff)|*.tif;*.tiff";
	public const string DIALOG_FILTER_BMP = "Bitmap (*.bmp)|*.bmp";
	
	// Game-specific filters
	public const string DIALOG_FILTER_BYTES = "Distance Serialized Binary File (*.bytes)|*.bytes";

	public const string DIALOG_MESSAGE_EDITOR_PREVIEW = "This software is currently still work in progress!\nPlease only use it for testing purposes and feedback as many breaking changes may occur in the future.";
	public const string DIALOG_MESSAGE_UNSAVED_CHANGES = "The currently opened file has unsaved changes!\nContinue without saving?";
	public const string DIALOG_MESSAGE_UNSAVED_CHANGES_CONTINUE = "You must save your changes before you continue!\nDo you want to save now?";
	public const string DIALOG_MESSAGE_REMOVE_PLAYLIST = "Are you sure you want to remove the following playlist: \"{0}\" ?";
	public const string DIALOG_MESSAGE_REMOVE_RESOURCE = "Are you sure you want to remove this resource ?\nObjects requiring this resource will need to be updated manually !";
	public const string DIALOG_MESSAGE_MISSING_GAME_EXE = "Could not locate the game executable when starting preview mode!\nPlease locate it in the settings (File > Settings).";
	public const string DIALOG_MESSAGE_ERROR_GAME_EXE = "An error occured when attempting to run the game!\n\nMake sure the following conditions are met:\n- The executable exists\n- The file is marked as executable (linux)\n- You have the adequate user permissions\n- You have enough system resources available\n\nTo set the game path, open the settings (File > Settings)";
	public const string DIALOG_MESSAGE_GAME_EXE_NOT_SET = "The game executable path is currently not defined, the current operation cannot continue.\n\nDo you want to open the settings to set it?";

	public const string DIALOG_CAPTION_EDITOR_PREVIEW = "Campaign Editor - Preview";
	public const string DIALOG_CAPTION_APP_CLOSE = "Close application";
	public const string DIALOG_CAPTION_NEW_FILE = "New file";
	public const string DIALOG_CAPTION_CLOSE_PROJECT = "Close project";
	public const string DIALOG_CAPTION_REMOVE_PLAYLIST = "Remove playlist";
	public const string DIALOG_CAPTION_REMOVE_RESOURCE = "Remove resource";
	public const string DIALOG_CAPTION_START_PREVIEW = "Preview mode";

	public const string PLAYLIST_NO_NAME = "(No playlist name)";

	public const int THUMBNAIL_WIDTH = 640;
	public const int THUMBNAIL_HEIGHT = 360;
	public const int THUMBNAIL_WIDTH_SMALL = THUMBNAIL_WIDTH / 4;
	public const int THUMBNAIL_HEIGHT_SMALL = THUMBNAIL_HEIGHT / 4;
	public const double THUMBNAIL_RATIO = THUMBNAIL_WIDTH / THUMBNAIL_HEIGHT;

	public static readonly Dictionary<ResourceType, Func<CampaignResource, CampaignResource>> RESOURCE_DIALOGS = new Dictionary<ResourceType, Func<CampaignResource, CampaignResource>>()
	{
		{ ResourceType.Level, (data) => new LevelDialog(data as CampaignResource.Level).ShowModal() },
		{ ResourceType.Texture, (data) => new TextureDialog(data as CampaignResource.Texture).ShowModal() }
	};
}
