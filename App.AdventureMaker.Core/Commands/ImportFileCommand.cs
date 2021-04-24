using App.AdventureMaker.Core.Forms;
using App.AdventureMaker.Core.Interfaces;
using App.AdventureMaker.Core.Tasks;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.IO;
using static Constants;
using static Dialogs;

namespace App.AdventureMaker.Core.Commands
{
	public class ImportFileCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;
		private readonly OpenFileDialog fileDialog;
		private readonly SelectFolderDialog folderDialog;
		private readonly Control owner;

		public ImportFileCommand(IEditor<CampaignFile> editor, Control owner)
		{
			this.editor = editor;
			this.owner = owner;

			MenuText = "&Import";
			ToolBarText = "Import";
			Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.I;

			fileDialog = ImportCampaignDialog("Import campaign package...");
			folderDialog = new SelectFolderDialog()
			{
				Title = "Select the destination folder to extract the files"
			};

			Enabled = false;
			editor.OnLoaded += (_) => Enabled = editor.CurrentFile != null;
		}

		protected override async void OnExecuted(EventArgs e)
		{
			if (fileDialog.ShowDialog(owner) == DialogResult.Ok && folderDialog.ShowDialog(owner) == DialogResult.Ok)
			{
				ImportProjectTask task = new ImportProjectTask(editor, new FileInfo(fileDialog.FileName), new DirectoryInfo(folderDialog.Directory));
				ProgressWindow progressWindow = new ProgressWindow(owner);
				await TaskBase.Run(progressWindow, task).ConfigureAwait(false);
				if (!task.GetResult())
				{
					Messages.InvalidFileDialog(new FileInfo(fileDialog.FileName), task.GetError());
				}
				else
				{
					Application.Instance.Invoke(() =>
					{
						editor.LoadFile(Path.Combine(folderDialog.Directory, "project.json"));
					});
				}
			}
		}
	}
}
