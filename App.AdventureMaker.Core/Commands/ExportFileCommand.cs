using App.AdventureMaker.Core.Forms.FileChecker;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Validation.Validators;
using Eto.Forms;
using System;
using System.IO;
using static Constants;
using static Dialogs;

namespace App.AdventureMaker.Core.Commands
{
	public class ExportFileCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;
		private readonly SaveFileDialog dialog;

		public ExportFileCommand(IEditor<CampaignFile> editor)
		{
			this.editor = editor;

			MenuText = "&Export";
			ToolBarText = "Export";
			Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.E;

			dialog = ExportCampaignDialog("Export campaign as...");

			Enabled = false;
			editor.OnLoaded += (_) => Enabled = editor.CurrentFile != null;
		}

		protected override void OnExecuted(EventArgs e)
		{
			if (dialog.ShowDialog(null) == DialogResult.Ok)
			{
				CampaignValidator validator = new CampaignValidator(editor.CurrentFile.Directory);
				validator.Validate(editor.Document);

				if (validator.GetMessages(StatusLevel.Error).Length > 0)
				{
					new FileCheckWindow(validator)
					{
						Title = DIALOG_CAPTION_EXPORT_CANCELED
					}.Show();
				}
				else
				{
				}
				Project.ExportProject(new FileInfo(dialog.FileName), editor);
			}
		}
	}
}
