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
			return MessageBox.Show(string.Format(Constants.DIALOG_MESSAGE_REMOVE_PLAYLIST, playlist.name), Constants.DIALOG_CAPTION_REMOVE_PLAYLIST, MessageBoxButtons.YesNo, MessageBoxType.Warning);
		}

		public static DialogResult RemoveResource()
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_REMOVE_RESOURCE, Constants.DIALOG_CAPTION_REMOVE_RESOURCE, MessageBoxButtons.YesNo, MessageBoxType.Warning);
		}

		public static DialogResult PreviewMessage()
		{
			//return MessageBox.Show(Constants.DIALOG_MESSAGE_EDITOR_PREVIEW, Constants.DIALOG_CAPTION_EDITOR_PREVIEW, MessageBoxButtons.OK, MessageBoxType.Information);
			return DialogResult.Ok;
		}
	}
}
