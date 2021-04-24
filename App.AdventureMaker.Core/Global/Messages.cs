using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.IO;
using System.Text;
using static Constants;

namespace App.AdventureMaker.Core
{
	public static class Messages
	{
		public static DialogResult UnsavedChangesDialog(string caption)
		{
			return MessageBox.Show(DIALOG_MESSAGE_UNSAVED_CHANGES, caption, MessageBoxButtons.YesNo, MessageBoxType.Warning);
		}

		public static DialogResult SaveChangesDialog(string caption)
		{
			return MessageBox.Show(DIALOG_MESSAGE_UNSAVED_CHANGES, caption, MessageBoxButtons.YesNo, MessageBoxType.Warning);
		}

		public static DialogResult RemovePlaylistDialog(CampaignPlaylist playlist)
		{
			return MessageBox.Show(string.Format(DIALOG_MESSAGE_REMOVE_PLAYLIST, playlist.Name), DIALOG_CAPTION_REMOVE_PLAYLIST, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult RemoveLevelDialog(CampaignLevel level)
		{
			return MessageBox.Show(string.Format(DIALOG_MESSAGE_REMOVE_LEVEL, level.Name), DIALOG_CAPTION_REMOVE_LEVEL, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult RemoveResourceDialog()
		{
			return MessageBox.Show(DIALOG_MESSAGE_REMOVE_RESOURCE, DIALOG_CAPTION_REMOVE_RESOURCE, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult ApplicationPreviewDialog()
		{
			return MessageBox.Show(DIALOG_MESSAGE_EDITOR_PREVIEW, DIALOG_CAPTION_EDITOR_PREVIEW, MessageBoxButtons.OK, MessageBoxType.Information);
		}

		public static DialogResult SaveBeforeContinueDialog()
		{
			return MessageBox.Show(DIALOG_MESSAGE_UNSAVED_CHANGES_CONTINUE, DIALOG_CAPTION_START_PREVIEW, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult MissingGameExeDialog()
		{
			return MessageBox.Show(DIALOG_MESSAGE_MISSING_GAME_EXE, DIALOG_CAPTION_START_PREVIEW, MessageBoxButtons.OK, MessageBoxType.Error);
		}

		public static DialogResult GameExeErrorDialog()
		{
			return MessageBox.Show(DIALOG_MESSAGE_ERROR_GAME_EXE, DIALOG_CAPTION_START_PREVIEW, MessageBoxButtons.OK, MessageBoxType.Error);
		}

		public static DialogResult GameExeNotSetDialog()
		{
			return MessageBox.Show(DIALOG_MESSAGE_GAME_EXE_NOT_SET, DIALOG_CAPTION_START_PREVIEW, MessageBoxButtons.YesNo, MessageBoxType.Question);
		}

		public static DialogResult InvalidFileDialog(FileInfo file, Exception error)
		{
			StringBuilder errorString = new StringBuilder();
			errorString.AppendLine(error.GetType().FullName);
			errorString.AppendLine(error.Message);

			return MessageBox.Show(string.Format(DIALOG_MESSAGE_INVALID_FILE, file.FullName, errorString.ToString()), DIALOG_CAPTION_INVALID_FILE, MessageBoxButtons.OK, MessageBoxType.Error);
		}
	}
}
