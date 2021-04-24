using App.AdventureMaker.Core.Forms;
using App.AdventureMaker.Core.Forms.FileChecker;
using App.AdventureMaker.Core.Interfaces;
using App.AdventureMaker.Core.Tasks;
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
		private readonly Control owner;

		public ExportFileCommand(IEditor<CampaignFile> editor, Control owner)
		{
			this.editor = editor;
			this.owner = owner;

			MenuText = "&Export";
			ToolBarText = "Export";
			Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.E;

			dialog = ExportCampaignDialog("Export campaign as package...");

			Enabled = false;
			editor.OnLoaded += (_) => Enabled = editor.CurrentFile != null;
		}

		protected override async void OnExecuted(EventArgs e)
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
			else if (dialog.ShowDialog(owner) == DialogResult.Ok)
			{
				ExportProjectTask task = new ExportProjectTask(editor, new FileInfo(dialog.FileName));
				ProgressWindow progressWindow = new ProgressWindow(owner);
				await TaskBase.Run(progressWindow, task).ConfigureAwait(false);
			}
		}
	}
}
