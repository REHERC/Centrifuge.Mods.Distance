using Distance.AdventureMaker.Common.Models;
using Eto.Forms;

namespace App.AdventureMaker.Core
{
	public static class Messages
	{
		public static DialogResult UnsavedChangesDialog(string caption)
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_UNSAVED_CHANGES, caption, MessageBoxButtons.YesNo, MessageBoxType.Warning);
		}

		public static DialogResult RemovePlaylist(CampaignPlaylist playlist)
		{
			return MessageBox.Show(string.Format(Constants.DIALOG_MESSAGE_REMOVE_PLAYLIST, playlist.Name), Constants.DIALOG_CAPTION_REMOVE_PLAYLIST, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult RemoveLevel(CampaignLevel level)
		{
			return MessageBox.Show(string.Format(Constants.DIALOG_MESSAGE_REMOVE_LEVEL, level.Name), Constants.DIALOG_CAPTION_REMOVE_LEVEL, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult RemoveResource()
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_REMOVE_RESOURCE, Constants.DIALOG_CAPTION_REMOVE_RESOURCE, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult PreviewMessage()
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_EDITOR_PREVIEW, Constants.DIALOG_CAPTION_EDITOR_PREVIEW, MessageBoxButtons.OK, MessageBoxType.Information);
			//return DialogResult.Ok;
		}

		public static DialogResult SaveBeforeContinue()
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_UNSAVED_CHANGES_CONTINUE, Constants.DIALOG_CAPTION_START_PREVIEW, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult MissingGameExe()
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_MISSING_GAME_EXE, Constants.DIALOG_CAPTION_START_PREVIEW, MessageBoxButtons.OK, MessageBoxType.Error);
		}

		public static DialogResult GameExeError()
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_ERROR_GAME_EXE, Constants.DIALOG_CAPTION_START_PREVIEW, MessageBoxButtons.OK, MessageBoxType.Error);
		}

		public static DialogResult GameExeNotSet()
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_GAME_EXE_NOT_SET, Constants.DIALOG_CAPTION_START_PREVIEW, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}
	}
}
