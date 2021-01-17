using Eto.Forms;

namespace App.AdventureMaker.Core
{
	public static class Messages
	{
		public static DialogResult UnsavedChangesDialog(string caption)
		{
			return MessageBox.Show(Constants.DIALOG_MESSAGE_UNSAVED_CHANGES, caption, MessageBoxButtons.YesNo, MessageBoxType.Warning);
		}
	}
}
