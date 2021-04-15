using App.AdventureMaker.Core.Forms.FileChecker;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Validation.Validators;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Commands
{
	public class CheckFilesCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;

		public CheckFilesCommand(IEditor<CampaignFile> editor)
		{
			this.editor = editor;

			MenuText = "&Check Files";
			ToolBarText = "Check Files";
			Image = Resources.GetIcon("FolderCheck.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.F6;

			Enabled = false;
			editor.OnLoaded += (_) => Enabled = editor.CurrentFile != null;
		}

		protected override void OnExecuted(EventArgs e)
		{
			CampaignValidator validator = new CampaignValidator(editor.CurrentFile.Directory);
			validator.Validate(editor.Document);
			new FileCheckWindow(validator).Show();
		}
	}
}
