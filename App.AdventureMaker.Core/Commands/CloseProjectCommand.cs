using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.IO;

namespace App.AdventureMaker.Core.Commands
{
	public class CloseProjectCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;

		public CloseProjectCommand(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			MenuText = "&Close project";
			ToolBarText = "Close project";
			Image = Resources.GetIcon("Close.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.W;

			Enabled = false;

			editor.OnLoaded += (_) => Enabled = editor.CurrentFile != null;
		}

		protected override void OnExecuted(EventArgs e)
		{
			if (editor.Modified && Messages.UnsavedChangesDialog(Constants.DIALOG_CAPTION_CLOSE_PROJECT) == DialogResult.No)
			{
				return;
			}

			RecentProjects.Update(editor.CurrentFile);

			editor.LoadFile(null as FileInfo);
		}
	}
}
